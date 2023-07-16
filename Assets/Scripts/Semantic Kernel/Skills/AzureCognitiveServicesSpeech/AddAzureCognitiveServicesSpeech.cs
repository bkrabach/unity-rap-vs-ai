using Unity.VisualScripting;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel/Skills/AzureCognitiveServicesSpeech")]
    [UnitTitle("Add Azure Cognitive Services Speech skill")]
    [UnitSubtitle("Add Azure Cognitive Services Speech skill to kernel")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class AddAzureCognitiveServicesSpeech : Unit
    {
        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        public ValueInput subscriptionKey;

        [DoNotSerialize]
        public ValueInput serviceRegion;

        [DoNotSerialize]
        public ValueInput voiceName;

        [DoNotSerialize]
        public ValueInput speechRate;

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
            subscriptionKey = ValueInput<string>("subscriptionKey");
            serviceRegion = ValueInput<string>("serviceRegion");
            voiceName = ValueInput<string>("voiceName", string.Empty);
            speechRate = ValueInput<string>("speechRate", string.Empty);

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput("output", (flow) =>
            {
                var kernelValue = flow.GetValue<IKernel>(kernel);
                var subscriptionKeyValue = flow.GetValue<string>(subscriptionKey);
                var serviceRegionValue = flow.GetValue<string>(serviceRegion);
                var voiceNameValue = flow.GetValue<string>(voiceName);
                var speechRateValue = flow.GetValue<string>(speechRate);

                var speechSkill = new AzureCognitiveServicesSpeechSkill(subscriptionKeyValue, serviceRegionValue, voiceNameValue, speechRateValue);

                var speechFunction = kernelValue.ImportSkill(speechSkill, "speech");
                return speechFunction;
            });

            Requirement(kernel, enter);
            Requirement(voiceName, enter);
            Requirement(speechRate, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
