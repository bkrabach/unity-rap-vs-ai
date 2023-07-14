using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.VisualScripting;
using Microsoft.SemanticKernel.Orchestration;

namespace Microsoft.SemanticKernel.Unity.VisualScripting
{
    [UnitCategory("Semantic Kernel")]
    [UnitTitle("Kernel Run Async (kernel, input, function")]
    [UnitSubtitle("Run a function")]
    [UnitShortTitle("Kernel Run Async")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class KernelRunAsyncFunction : WaitUnit
    {
        [DoNotSerialize]
        public ValueInput kernel;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput input;

        [DoNotSerialize]
        [AllowsNull]
        public ValueInput variables;

        [DoNotSerialize]
        public ValueInput function;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput output;

        private SKContext runResult;

        protected override void Definition()
        {
            base.Definition();

            kernel = ValueInput<IKernel>("kernel");
            input = ValueInput<string>("input", null);
            variables = ValueInput<ContextVariables>("variables", null);
            function = ValueInput<ISKFunction>("function");
            
            output = ValueOutput<SKContext>("output", (flow) =>
            {
                if (runResult == null)
                {
                    throw new Exception("no result available");
                }
                return runResult;
            });

            Requirement(kernel, enter);
            Requirement(function, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var kernelValue = flow.GetValue<IKernel>(kernel);
            var inputValue = input.hasValidConnection ? flow.GetValue<string>(input) : null;
            var variablesValue = variables.hasValidConnection ? flow.GetValue<ContextVariables>(variables) : null;
            var functionValue = flow.GetValue<ISKFunction>(function);
            var pipeline = new ISKFunction[]
            {
                functionValue
            };

            var task = Task.Run(async () =>
            {
                if (inputValue != null) {
                    runResult = await kernelValue.RunAsync(inputValue, pipeline);
                }
                else if (variablesValue != null)
                {
                    runResult = await kernelValue.RunAsync(variablesValue, pipeline);
                }
                else
                {
                    runResult = await kernelValue.RunAsync(pipeline);
                }
            });

            yield return new WaitUntil(() => task.IsCompleted);
            yield return exit;
        }
    }
}
