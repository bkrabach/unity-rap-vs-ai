using Unity.VisualScripting;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Connectors/AI/OpenAI")]
    [UnitTitle("Azure Text Completion")]
    [UnitSubtitle("Add Azure text completion service")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class AddAzureTextCompletionService : Unit
    {
        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        public ValueInput serviceId;

        [DoNotSerialize]
        public ValueInput deploymentName;

        [DoNotSerialize]
        public ValueInput endpoint;

        [DoNotSerialize]
        public ValueInput apiKey;

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
            deploymentName = ValueInput<string>("deploymentName", string.Empty);
            endpoint = ValueInput<string>("endpoint", string.Empty);
            apiKey = ValueInput<string>("apiKey");

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<IKernel>("output", (flow) =>
            {
                var kernelValue = flow.GetValue<IKernel>(kernel);
                var serviceIdValue = flow.GetValue<string>(serviceId);
                var deploymentNameValue = flow.GetValue<string>(deploymentName);
                var endpointValue = flow.GetValue<string>(endpoint);
                var apiKeyValue = flow.GetValue<string>(apiKey);
                
                kernelValue.Config.AddAzureTextCompletionService(
                    serviceIdValue,
                    deploymentNameValue,
                    endpointValue,
                    apiKeyValue
                );
                return kernelValue;
            });

            Requirement(kernel, enter);
            Requirement(serviceId, enter);
            Requirement(deploymentName, enter);
            Requirement(endpoint, enter);
            Requirement(apiKey, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
