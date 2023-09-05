﻿using System.Linq;
using ToSic.Eav.Code.Help;
using ToSic.Eav.Plumbing;

namespace ToSic.Sxc.Code.Help
{
    internal class GenChangeOn : GenNotExist
    {

        public GenChangeOn(string fullNamespace, string name, string alt) : this(fullNamespace, name, new[] { (alt, null as string) })
        {
        }

        public GenChangeOn(string fullNamespace, string name, (string Code, string Comment)[] alt) : base(name, alt)
        {
            FullNameSpace = fullNamespace;
        }

        public readonly string FullNameSpace;
        public string MsgWhichWasCommon;
        public string NotOn;

        protected override string HtmlRecommendations() => Alt.Length == 1
            ? HtmlRec(("." + Alt[0].Code, Alt[0].Comment))
            : $"<ol>{string.Join("\n", Alt.Select(a => HtmlRec(("." + a.Code, a.Comment))))}</ol>";

        public override CodeHelp Generate()
        {
            return new CodeHelp(name: $"{FullNameSpace}-{Name}-DoesNotExist",
                detect: DetectTypeDoesNotContain(FullNameSpace, Name),
                linkCode: LinkCode,
                uiMessage: $@"
You are calling the '{Name}' property {MsgWhichWasCommon}, but not available on {NotOn} (RazorTyped). {Comments}
You should probably use '{Alt[0].Code}' {Alt[0].Comment}
",
                detailsHtml: $@"
You are probably calling <code>.{Name}</code>.
{(Comments.HasValue() ? $"<br><em>{Comments}</em><br>" : "")}
The property <code>.{Name}</code> is replaced with: 
{HtmlRecommendations()}
"
            );

        }

    }
    
}
