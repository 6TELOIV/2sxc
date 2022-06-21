﻿using ToSic.Eav.Documentation;

namespace ToSic.Sxc.Edit.Toolbar
{
    public partial class ToolbarBuilder
    {
        /// <inheritdoc />
        public IToolbarBuilder Metadata(
            object target,
            string contentTypes = null,
            string noParamOrder = Eav.Parameters.Protector,
            string ui = null,
            string parameters = null,
            string context = null
        )
        {
            var finalTypes = GetMetadataTypeNames(target, contentTypes);
            var realContext = GetContext(target, context);
            var result = this as IToolbarBuilder;
            foreach (var type in finalTypes)
                result = result.Add(new ToolbarRuleMetadata(target, type, ui, parameters, context: realContext, helper: _deps.ToolbarButtonHelper.Ready));

            return result;
        }

        /// <inheritdoc />
        public IToolbarBuilder Copy(
            object target,
            string noParamOrder = Eav.Parameters.Protector,
            string ui = null,
            string parameters = null,
            string context = null
        ) => Add(new ToolbarRuleCopy(target, ui, parameters, GetContext(target, context), _deps.ToolbarButtonHelper.Ready));


        [PrivateApi("WIP 13.11")]
        public IToolbarBuilder Image(
            object target,
            string noParamOrder = Eav.Parameters.Protector,
            string ui = null,
            string parameters = null
        ) => Add(new ToolbarRuleImage(target, ui, parameters, context: GetContext(target, null), helper: _deps.ToolbarButtonHelper.Ready));
    }
}
