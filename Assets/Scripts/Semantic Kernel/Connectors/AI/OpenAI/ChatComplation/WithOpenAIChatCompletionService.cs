using Unity.VisualScripting;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Connectors/AI/OpenAI")]
    [UnitTitle("With OpenAI Chat Completion Service")]
    [UnitSubtitle("Add OpenAI chat completion service to builder")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class WithOpenAIChatCompletionService : Unit
    {
        [DoNotSerialize]
        public ValueInput kernelBuilder;

        [DoNotSerialize]
        public ValueInput modelId;

        [DoNotSerialize]
        public ValueInput apiKey;

        [DoNotSerialize]
        public ValueInput orgId;

        [DoNotSerialize]
        public ValueInput serviceId;

        [DoNotSerialize]
        public ValueInput alsoAsTextCompletion;

        [DoNotSerialize]
        public ValueInput setAsDefault;

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
            kernelBuilder = ValueInput<KernelBuilder>("kernelBuilder");
            modelId = ValueInput<string>("modelId", string.Empty);
            apiKey = ValueInput<string>("apiKey");
            orgId = ValueInput<string>("orgId", string.Empty);
            serviceId = ValueInput<string>("serviceId", string.Empty);
            alsoAsTextCompletion = ValueInput<bool>("alsoAsTextCompletion", true);
            setAsDefault = ValueInput<bool>("setAsDefault", false);

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<KernelBuilder>("output", (flow) =>
            {
                var kernelBuilderValue = flow.GetValue<KernelBuilder>(kernelBuilder);
                var modelIdValue = flow.GetValue<string>(modelId);
                var apiKeyValue = flow.GetValue<string>(apiKey);
                var orgIdValue = flow.GetValue<string>(orgId);
                var serviceIdValue = flow.GetValue<string>(serviceId);
                var alsoAsTextCompletionValue = flow.GetValue<bool>(alsoAsTextCompletion);
                var setAsDefaultValue = flow.GetValue<bool>(setAsDefault);

                kernelBuilderValue.WithOpenAIChatCompletionService(
                    modelIdValue,
                    apiKeyValue,
                    orgIdValue,
                    serviceIdValue,
                    alsoAsTextCompletionValue,
                    setAsDefaultValue
                );
                return kernelBuilderValue;
            });

            Requirement(kernelBuilder, enter);
            Requirement(modelId, enter);
            Requirement(apiKey, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
