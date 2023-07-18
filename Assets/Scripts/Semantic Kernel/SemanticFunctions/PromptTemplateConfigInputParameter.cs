using Unity.VisualScripting;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace Scripts.SemanticKernel.SemanticFunctions
{
    [UnitCategory("Semantic Kernel/Semantic Functions")]
    [UnitTitle("Input Parameter")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class PromptTemplateConfigInputParameter : Unit
    {
        [DoNotSerialize]
        public ValueInput name;
        
        [DoNotSerialize]
        public ValueInput description;

        [DoNotSerialize]
        public ValueInput defaultValue;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        protected override void Definition()
        {
            name = ValueInput<string>("name", string.Empty);
            description = ValueInput<string>("description", string.Empty);
            defaultValue = ValueInput<string>("defaultValue", string.Empty);

            output = ValueOutput<PromptTemplateConfig.InputParameter>("output", (flow) =>
            {
                var nameValue = flow.GetValue<string>(name);
                var descriptionValue = flow.GetValue<string>(description);
                var defaultValueValue = flow.GetValue<string>(defaultValue);

                var inputParameter = new PromptTemplateConfig.InputParameter
                {
                    Name = nameValue,
                    Description = descriptionValue,
                    DefaultValue = defaultValueValue
                };
                return inputParameter;
            });
        }
    }
}
