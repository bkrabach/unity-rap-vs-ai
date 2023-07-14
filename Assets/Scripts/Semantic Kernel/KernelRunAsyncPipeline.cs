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
    [UnitTitle("Kernel Run Async (kernel, input, pipeline)")]
    [UnitSubtitle("Run a pipeline of functions")]
    [UnitShortTitle("Kernel Run Async")]
    [UnitSurtitle("Semantic Kernel")]
    public sealed class KernelRunAsyncPipeline : WaitUnit
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
        public ValueInput pipeline;

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
            pipeline = ValueInput<ISKFunction[]>("pipeline");
            
            output = ValueOutput<SKContext>("output", (flow) =>
            {
                if (runResult == null)
                {
                    throw new Exception("no result available");
                }
                return runResult;
            });

            Requirement(kernel, enter);
            Requirement(pipeline, enter);
            Succession(enter, exit);
            Assignment(enter, output);
        }

        protected override IEnumerator Await(Flow flow)
        {
            var kernelValue = flow.GetValue<IKernel>(kernel);
            var inputValue = input.hasValidConnection ? flow.GetValue<string>(input) : null;
            var variablesValue = variables.hasValidConnection ? flow.GetValue<ContextVariables>(variables) : null;
            var pipelineValue = flow.GetValue<ISKFunction[]>(pipeline);
            
            var task = Task.Run(async () =>
            {
                if (inputValue != null) {
                    runResult = await kernelValue.RunAsync(inputValue, pipelineValue);
                }
                else if (variablesValue != null)
                {
                    runResult = await kernelValue.RunAsync(variablesValue, pipelineValue);
                }
                else
                {
                    runResult = await kernelValue.RunAsync(pipelineValue);
                }
            });

            yield return new WaitUntil(() => task.IsCompleted);
            yield return exit;
        }
    }
}
