using Unity.VisualScripting;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Connectors/AI/OpenAI")]
    [UnitTitle("OpenAI Chat Completion")]
    [UnitSubtitle("Add OpenAI chat completion service")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class AddOpenAIChatCompletionService : Unit
    {
        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        public ValueInput serviceId;

        [DoNotSerialize]
        public ValueInput modelId;

        [DoNotSerialize]
        public ValueInput apiKey;

        [DoNotSerialize]
        public ValueInput orgId;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput enter;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput exit;

        protected override void Definition()
        {
            kernel = ValueInput<IKernel>("kernel");
            serviceId = ValueInput<string>("serviceId", string.Empty);
            modelId = ValueInput<string>("modelId", string.Empty);
            apiKey = ValueInput<string>("apiKey");
            orgId = ValueInput<string>("orgId", string.Empty);

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<IKernel>("output", (flow) =>
            {
                var kernelValue = flow.GetValue<IKernel>(kernel);
                var serviceIdValue = flow.GetValue<string>(serviceId);
                var modelIdValue = flow.GetValue<string>(modelId);
                var apiKeyValue = flow.GetValue<string>(apiKey);
                var orgIdValue = flow.GetValue<string>(orgId);

                kernelValue.Config.AddOpenAIChatCompletionService(
                    serviceIdValue,
                    modelIdValue,
                    apiKeyValue,
                    orgIdValue
                );
                return kernelValue;
            });

            Requirement(kernel, enter);
            Requirement(serviceId, enter);
            Requirement(modelId, enter);
            Requirement(apiKey, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
