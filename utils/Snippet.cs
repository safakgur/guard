namespace Dawn.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Dawn;

    internal abstract class Snippet
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false);

        public Snippet(MethodInfo method, GuardFunctionAttribute attribute, string shortcut)
        {
            this.Method = method;
            this.Attribute = attribute;
            this.Shortcut = shortcut;
        }

        public MethodInfo Method { get; }

        public GuardFunctionAttribute Attribute { get; }

        public string Shortcut { get; }

        public static Task SaveSnippets(string directory, CancellationToken ct)
        {
            var groups = GuardFunctionAttribute.GetMethods(Assembly.GetAssembly(typeof(Guard)))
                .Where(p => p.Value.Shortcut != null)
                .GroupBy(p => p.Value.Shortcut)
                .ToList();

            return SaveVisualStudioSnippets("guard-cs.vs.snippet");

            async Task SaveVisualStudioSnippets(string name)
            {
                var snippets = groups.SelectMany(g =>
                    g.SelectMany(p => VisualStudioSnippet.CreateSnippets(p.Key, p.Value)));

                var document = VisualStudioSnippet.CreateDocument(snippets);
                using (var file = File.Create(Path.Combine(directory, name)))
                using (var writer = new StreamWriter(file, Encoding))
                {
                    await document.SaveAsync(writer, SaveOptions.None, ct).ConfigureAwait(false);
                    Console.WriteLine($"Created \"{file.Name}\"");
                }
            }
        }

        public sealed class VisualStudioSnippet : Snippet
        {
            private static readonly XDeclaration Declaration = new XDeclaration("1.0", "utf-8", "no");

            private static readonly XNamespace Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

            private readonly XElement element;

            private VisualStudioSnippet(
                MethodInfo method, GuardFunctionAttribute attribute, string shortcut, XElement element)
                : base(method, attribute, shortcut)
                => this.element = element;

            public static IEnumerable<VisualStudioSnippet> CreateSnippets(MethodInfo method, GuardFunctionAttribute attribute)
            {
                var isExtMethod = method.GetCustomAttribute<ExtensionAttribute>() != null;
                var isStateGuard = attribute.Group.Equals("State", StringComparison.OrdinalIgnoreCase);
                var typeParams = method.IsGenericMethod ? method.GetGenericArguments() : Array.Empty<Type>();
                var allParams = method.GetParameters();
                var listParams = allParams
                    .Skip(isExtMethod ? 1 : 0)
                    .Where(p => p.Name != "message" && p.GetCustomAttribute<CallerMemberNameAttribute>() is null)
                    .ToList();

                var element = CreateElement(attribute.Shortcut, false);
                yield return new VisualStudioSnippet(method, attribute, attribute.Shortcut, element);

                if (isStateGuard ||
                    method.Name == nameof(Guard.Argument) ||
                    method.Name == nameof(Guard.Null) ||
                    method.Name == nameof(Guard.NotNull))
                    yield break;

                var canBeNull = !isExtMethod;
                if (!canBeNull)
                {
                    var constraints = method.GetGenericArguments().FirstOrDefault()?.GetGenericParameterConstraints();
                    if (constraints != null)
                    {
                        canBeNull = !constraints.Contains(typeof(ValueType));
                        if (!canBeNull)
                        {
                            var argumentType = allParams[0].ParameterType.GetGenericArguments()[0];
                            canBeNull = argumentType.IsGenericType && argumentType.GetGenericTypeDefinition() == typeof(Nullable<>);
                        }
                    }
                    else
                    {
                        var argumentType = allParams[0].ParameterType.GetGenericArguments()[0];

                        canBeNull = !argumentType.IsValueType;
                        if (!canBeNull && argumentType.IsGenericType && argumentType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            canBeNull = true;
                    }
                }

                if (canBeNull)
                {
                    var shortcut = attribute.Shortcut.Insert(1, "x");
                    element = CreateElement(shortcut, true);
                    yield return new VisualStudioSnippet(method, attribute, shortcut, element);
                }

                XElement CreateElement(string shortcut, bool addNullGuard)
                {
                    var title = string.Format(
                        "{0}{1}.{2}{3}({4})",
                        method.DeclaringType.Name,
                        addNullGuard ? ".NotNull()" : string.Empty,
                        method.Name,
                        !isExtMethod && typeParams.Length > 0
                            ? $"`{typeParams.Length}"
                            : string.Empty,
                        listParams.Count > 0
                            ? string.Join(", ", listParams.Select(p => p.Name.Replace("&", string.Empty)))
                            : string.Empty);

                    var code = new StringBuilder("Guard");
                    if (!isStateGuard)
                        code.Append(".Argument($arg$, nameof($arg$))");

                    if (method.Name == nameof(Guard.Argument))
                    {
                        code.Append("$end$");
                    }
                    else
                    {
                        if (addNullGuard)
                            code.Append(".NotNull()");

                        code.AppendFormat(".{0}", method.Name);
                        if (!isExtMethod && typeParams.Length > 0)
                            code.AppendFormat("<{0}>", string.Join(", ", typeParams.Select(t => $"${t.Name}$")));

                        code.Append("(");
                        code.Append(string.Join(", ", listParams.Select(p =>
                        {
                            if (p.ParameterType == typeof(StringComparison))
                                return $"StringComparison.${p.Name}$";

                            else if (p.ParameterType == typeof(StringComparer))
                                return $"StringComparer.${p.Name}$";

                            return $"${p.Name}$";
                        })));

                        code.AppendFormat(")$end$;");
                    }

                    return new XElement(
                        Namespace + "CodeSnippet",
                        new XAttribute("Format", "1.0.0"),
                        new XElement(
                            Namespace + "Header",
                            new XElement(Namespace + "Title", title),
                            new XElement(Namespace + "Shortcut", shortcut)),
                        new XElement(
                            Namespace + "Snippet",
                            new XElement(
                                Namespace + "Imports",
                                new XElement(
                                    Namespace + "Import",
                                    new XElement(Namespace + "Namespace", nameof(Dawn)))),
                            new XElement(
                                Namespace + "Declarations",
                                isStateGuard ? null : new XElement(
                                    Namespace + "Literal",
                                    new XElement(Namespace + "ID", "arg"),
                                    new XElement(Namespace + "Default", "arg")),
                                isExtMethod ? null : typeParams.Select(t => new XElement(
                                    Namespace + "Literal",
                                    new XElement(Namespace + "ID", t.Name),
                                    new XElement(Namespace + "Default", t.Name))),
                                listParams.Select(p => new XElement(
                                    Namespace + "Literal",
                                    new XElement(Namespace + "ID", p.Name),
                                    new XElement(Namespace + "Default", p.Name)))),
                            new XElement(
                                Namespace + "Code",
                                new XAttribute("Language", "csharp"),
                                new XCData(code.ToString()))));
                }
            }

            public static XDocument CreateDocument(IEnumerable<VisualStudioSnippet> snippets)
            {
                var elements = snippets
                    .GroupBy(s => s.Shortcut)
                    .Select(g => new XElement(g.First().element));

                return new XDocument(Declaration, new XElement(Namespace + "CodeSnippets", elements));
            }
        }
    }
}
