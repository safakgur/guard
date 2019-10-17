using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Dawn.Tests
{
    public sealed class AnnotationTests : BaseTests
    {
        [Fact(DisplayName = "Annotations: [GuardFunc] initialization")]
        public void GuardFunctionInit()
        {
            Assert.Throws<ArgumentNullException>("group", () => new GuardFunctionAttribute(null));
            Assert.Throws<ArgumentException>("group", () => new GuardFunctionAttribute(string.Empty));
            Assert.Throws<ArgumentException>("group", () => new GuardFunctionAttribute(" "));

            var attr = new GuardFunctionAttribute("G");
            Assert.Equal("G", attr.Group);
            Assert.Null(attr.Shortcut);
            Assert.Equal(0, attr.Order);

            Assert.Throws<ArgumentException>("shortcut", () => new GuardFunctionAttribute("G", string.Empty));
            Assert.Throws<ArgumentException>("shortcut", () => new GuardFunctionAttribute("G", " "));
            Assert.Throws<ArgumentException>("shortcut", () => new GuardFunctionAttribute("G", "s"));
            Assert.Throws<ArgumentException>("shortcut", () => new GuardFunctionAttribute("G", "g"));

            attr = new GuardFunctionAttribute("G", "gs");
            Assert.Equal("G", attr.Group);
            Assert.Equal("gs", attr.Shortcut);
            Assert.Equal(0, attr.Order);

            attr = new GuardFunctionAttribute("G", order: 1);
            Assert.Equal("G", attr.Group);
            Assert.Null(attr.Shortcut);
            Assert.Equal(1, attr.Order);

            attr = new GuardFunctionAttribute("G", "gs", order: 1);
            Assert.Equal("G", attr.Group);
            Assert.Equal("gs", attr.Shortcut);
            Assert.Equal(1, attr.Order);
        }

        [Fact(DisplayName = "Annotations: Exported methods are marked")]
        public void ExportedMethodsAreMarked()
        {
            var assembly = Assembly.GetAssembly(typeof(Guard));
            var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            var exportedMethods =
                from t in assembly.ExportedTypes
                where !t.FullName.StartsWith("Coverlet")
                   && t.GetCustomAttribute<ObsoleteAttribute>() is null
                select t.GetMethods(flags) into methods
                from m in methods
                where m.DeclaringType.Assembly == assembly
                   && !m.IsVirtual
                   && !m.IsSpecialName
                   && m.GetCustomAttribute<NonGuardAttribute>() is null
                   && m.GetCustomAttribute<ObsoleteAttribute>() is null
                select m;

            var markedMethods = GetMarkedMethods().Select(p => p.Key).ToHashSet();
            foreach (var e in exportedMethods)
                Assert.Contains(e, markedMethods);
        }

        [Fact(DisplayName = "Annotations: Shortcuts are unique")]
        public void ShortcutsAreUnique()
        {
            var groups = GetMarkedMethods()
                .Where(p => p.Value.Shortcut != null)
                .GroupBy(p => p.Value.Shortcut);

            foreach (var group in groups)
                Assert.False(group.GroupBy(t => t.Key.Name).Skip(1).Any());
        }

        private static IEnumerable<KeyValuePair<MethodInfo, GuardFunctionAttribute>> GetMarkedMethods()
            => GuardFunctionAttribute.GetMethods(Assembly.GetAssembly(typeof(Guard)));
    }
}
