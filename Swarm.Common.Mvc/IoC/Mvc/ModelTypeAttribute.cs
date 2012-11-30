using System;

namespace Swarm.Common.Mvc.IoC.Mvc
{
    /// <summary>
    /// Defines the model type a model binder is in charge of binding.
    /// </summary>
    public sealed class ModelTypeAttribute : Attribute
    {
        public Type ModelType { get; private set; }

        public ModelTypeAttribute(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            ModelType = modelType;
        }
    }
}
