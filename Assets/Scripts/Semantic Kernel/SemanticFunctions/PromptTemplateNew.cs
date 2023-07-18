using Unity.VisualScripting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;

namespace Scripts.SemanticKernel.SemanticFunctions
{
    [UnitCategory("Semantic Kernel/Semantic Functions")]
    [UnitTitle("Prompt Template")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class PromptTemplateNew : Unit
    {
        [DoNotSerialize]
        public ValueInput template;

        [DoNotSerialize]
        public ValueInput promptTemplateConfig;

        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        protected override void Definition()
        {
            template = ValueInput<string>("template");
            promptTemplateConfig = ValueInput<PromptTemplateConfig>("promptTemplateConfig");
            kernel = ValueInput<IKernel>("kernel");

            output = ValueOutput<PromptTemplate>("output", (flow) =>
            {
                var templateValue = flow.GetValue<string>(template);
                var promptTemplateConfigValue = flow.GetValue<PromptTemplateConfig>(promptTemplateConfig);
                var kernelValue = flow.GetValue<IKernel>(kernel);

                var promptTemplate = new PromptTemplate(templateValue, promptTemplateConfigValue, kernelValue);
                return promptTemplate;
            });
        }
    }
}
