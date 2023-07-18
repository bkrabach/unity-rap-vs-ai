using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Unity.VisualScripting;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

namespace Scripts.SemanticKernel.SemanticFunctions
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("Invoke Semantic Function")]
    [UnitSubtitle("Run a function on the input")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class SemanticFunctionInvoke : WaitUnit
    {
        [DoNotSerialize]
        public ValueInput function;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput input;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput context;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput settings;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        private SKContext invokeResult;

        protected override void Definition()
        {
            base.Definition();

            function = ValueInput<ISKFunction>("function");
            input = ValueInput<string>("input", null);
            context = ValueInput<SKContext>("context", null);
            settings = ValueInput<CompleteRequestSettings>("settings", null);

            output = ValueOutput<SKContext>("output", (flow) =>
            {
                if (invokeResult == null)
                {
                    throw new Exception("no result available");
                }
                return invokeResult;
            });

            Requirement(function, enter);
            Requirement(input, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var functionValue = flow.GetValue<ISKFunction>(function);
            var inputValue = input.hasValidConnection ? flow.GetValue<string>(input) : null;
            var contextValue = context.hasValidConnection ? flow.GetValue<SKContext>(context) : null;
            var settingsValue = settings.hasValidConnection ? flow.GetValue<CompleteRequestSettings>(settings) : null;

            var task = Task.Run(async () =>
            {
                invokeResult = await functionValue.InvokeAsync(contextValue, settingsValue);
                //if (inputValue == null)
                //{
                //    invokeResult = await functionValue.InvokeAsync(contextValue, settingsValue);
                //}
                //else
                //{
                //    invokeResult = await functionValue.InvokeAsync(inputValue, contextValue, settingsValue);
                //}
            });

            yield return new WaitUntil(() => task.IsCompleted);
            yield return exit;
        }
    }
}
