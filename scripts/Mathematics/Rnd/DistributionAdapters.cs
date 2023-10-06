namespace Craiel.Essentials.Mathematics.Rnd;

using System;
using System.Collections.Generic;
using Contracts;
using Utils;

public static class DistributionAdapters
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    static DistributionAdapters()
    {
        Adapters.Add(TypeDef<ConstantDoubleDistribution>.Value, new ConstantDoubleDistributionAdapter());
        Adapters.Add(TypeDef<ConstantFloatDistribution>.Value, new ConstantFloatDistributionAdapter());
        Adapters.Add(TypeDef<ConstantIntegerDistribution>.Value, new ConstantIntegerDistributionAdapter());
        Adapters.Add(TypeDef<ConstantLongDistribution>.Value, new ConstantLongDistributionAdapter());

        Adapters.Add(TypeDef<GaussianDoubleDistribution>.Value, new GaussianDoubleDistributionAdapter());
        Adapters.Add(TypeDef<GaussianFloatDistribution>.Value, new GaussianFloatDistributionAdapter());

        Adapters.Add(TypeDef<TriangularDoubleDistribution>.Value, new TriangularDoubleDistributionAdapter());
        Adapters.Add(TypeDef<TriangularFloatDistribution>.Value, new TriangularFloatDistributionAdapter());
        Adapters.Add(TypeDef<TriangularIntegerDistribution>.Value, new TriangularIntegerDistributionAdapter());
        Adapters.Add(TypeDef<TriangularLongDistribution>.Value, new TriangularLongDistributionAdapter());

        Adapters.Add(TypeDef<UniformDoubleDistribution>.Value, new UniformDoubleDistributionAdapter());
        Adapters.Add(TypeDef<UniformFloatDistribution>.Value, new UniformFloatDistributionAdapter());
        Adapters.Add(TypeDef<UniformIntegerDistribution>.Value, new UniformIntegerDistributionAdapter());
        Adapters.Add(TypeDef<UniformLongDistribution>.Value, new UniformLongDistributionAdapter());
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static readonly IDictionary<Type, DistributionAdapter> Adapters = new Dictionary<Type, DistributionAdapter>();

    public abstract class DoubleAdapter<TN> : DistributionAdapter
        where TN : DoubleDistribution
    {
        protected DoubleAdapter(string category)
            : base(category, TypeDef<DoubleDistribution>.Value)
        {
        }

        public override T ToDistribution<T>(params string[] parameters)
        {
            return (T)(IDistribution)this.ToDistributionTyped(parameters);
        }

        public override string[] ToParameters(IDistribution distribution)
        {
            return this.ToParametersTyped((TN)distribution);
        }

        protected abstract TN ToDistributionTyped(params string[] parameters);

        protected abstract string[] ToParametersTyped(TN distribution);
    }

    public abstract class FloatAdapter<TN> : DistributionAdapter
        where TN : FloatDistribution
    {
        protected FloatAdapter(string category)
            : base(category, TypeDef<FloatDistribution>.Value)
        {
        }

        public override T ToDistribution<T>(params string[] parameters)
        {
            return (T)(IDistribution)this.ToDistributionTyped(parameters);
        }

        public override string[] ToParameters(IDistribution distribution)
        {
            return this.ToParametersTyped((TN)distribution);
        }

        protected abstract TN ToDistributionTyped(params string[] parameters);

        protected abstract string[] ToParametersTyped(TN distribution);
    }

    public abstract class IntegerAdapter<TN> : DistributionAdapter
        where TN : IntegerDistribution
    {
        protected IntegerAdapter(string category)
            : base(category, TypeDef<IntegerDistribution>.Value)
        {
        }

        public override T ToDistribution<T>(params string[] parameters)
        {
            return (T)(IDistribution)this.ToDistributionTyped(parameters);
        }

        public override string[] ToParameters(IDistribution distribution)
        {
            return this.ToParametersTyped((TN)distribution);
        }

        protected abstract TN ToDistributionTyped(params string[] parameters);

        protected abstract string[] ToParametersTyped(TN distribution);
    }

    public abstract class LongAdapter<TN> : DistributionAdapter
        where TN : LongDistribution
    {
        protected LongAdapter(string category)
            : base(category, TypeDef<LongDistribution>.Value)
        {
        }

        public override T ToDistribution<T>(params string[] parameters)
        {
            return (T)(IDistribution)this.ToDistributionTyped(parameters);
        }

        public override string[] ToParameters(IDistribution distribution)
        {
            return this.ToParametersTyped((TN)distribution);
        }

        protected abstract TN ToDistributionTyped(params string[] parameters);

        protected abstract string[] ToParametersTyped(TN distribution);
    }
}
