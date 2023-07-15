using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Microsoft.SemanticKernel.SkillDefinition;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("Create Semantic Function (kernel, prompt)")]
    [UnitShortTitle("Create Semantic Function")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class KernelCreateSemanticFunction : Unit
    {
        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        public ValueInput prompt;
        
        [DoNotSerialize]
        [AllowsNull]
        public ValueInput functionName;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput skillName;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput description;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput maxTokens;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput temperature;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput topP;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput presencePenalty;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput frequencyPenalty;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput stopSequences;

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
            prompt = ValueInput<string>("prompt");
            functionName = ValueInput<string>("functionName", null);
            skillName = ValueInput<string>("skillName", string.Empty);
            description = ValueInput<string>("description", null);
            maxTokens = ValueInput<int>("maxTokens", 256);
            temperature = ValueInput<float>("temperature", 0);
            topP = ValueInput<float>("topP", 0);
            presencePenalty = ValueInput<float>("presencePenalty", 0);
            frequencyPenalty = ValueInput<float>("frequencyPenalty", 0);
            stopSequences = ValueInput<IEnumerable<string>>("stopSequences", null);

            enter = ControlInput("enter", (flow) => { return exit; });
            exit = ControlOutput("exit");

            output = ValueOutput<ISKFunction>("output", (flow) =>
            {
                var kernelValue = flow.GetValue<IKernel>(kernel);
                var promptValue = flow.GetValue<string>(prompt);
                var functionNameValue = flow.GetValue<string>(functionName);
                var skillNameValue = flow.GetValue<string>(skillName);
                var descriptionValue = flow.GetValue<string>(description);
                var maxTokensValue = flow.GetValue<int>(maxTokens);
                var temperatureValue = flow.GetValue<float>(temperature);
                var topPValue = flow.GetValue<float>(topP);
                var presencePenaltyValue = flow.GetValue<float>(presencePenalty);
                var frequencyPenaltyValue = flow.GetValue<float>(frequencyPenalty);
                var stopSequencesValue = stopSequences.hasValidConnection ? flow.GetValue<IEnumerable<string>>(stopSequences) : null;

                return kernelValue.CreateSemanticFunction(
                    promptValue,
                    functionNameValue,
                    skillNameValue,
                    descriptionValue,
                    maxTokensValue,
                    temperatureValue,
                    topPValue,
                    presencePenaltyValue,
                    frequencyPenaltyValue,
                    stopSequencesValue
                );
            });

            Requirement(kernel, enter);
            Requirement(prompt, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }
    }
}
