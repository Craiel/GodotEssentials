namespace Craiel.Essentials.Noise;

using Enums;
using Godot;

public partial class NoiseProvider
{
    private NoiseFractalType fractalType = NoiseFractalType.FBM;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public NoiseFractalType FractalType
    {
        get { return this.fractalType; }
        set { this.fractalType = value; }
    }
    
    // -------------------------------------------------------------------
    // Private
    // -------------------------------------------------------------------
    private float GetFractalFBM(Vector3 point)
    {
        int seedLocal = this.seed;
        float sum = this.GetBaseValue(seedLocal, point);
        float amp = 1;

        for (int i = 1; i < this.octaves; i++)
        {
            point.X *= this.lacunarity;
            point.Y *= this.lacunarity;
            point.Z *= this.lacunarity;

            amp *= this.gain;
            sum += this.GetBaseValue(++seedLocal, point) * amp;
        }

        return sum * this.fractalBounding;
    }
    
    private float GetFractalBillow(Vector3 point)
    {
        int seedLocal = this.seed;
        float sum = Mathf.Abs(this.GetBaseValue(seedLocal, point)) * 2 - 1;
        float amp = 1;

        for (int i = 1; i < this.octaves; i++)
        {
            point.X *= this.lacunarity;
            point.Y *= this.lacunarity;
            point.Z *= this.lacunarity;

            amp *= this.gain;
            sum += (Mathf.Abs(this.GetBaseValue(++seedLocal, point)) * 2 - 1) * amp;
        }

        return sum * this.fractalBounding;
    }

    private float GetFractalRigidMulti(Vector3 point)
    {
        int seedLocal = this.seed;
        float sum = 1 - Mathf.Abs(this.GetBaseValue(seedLocal, point));
        float amp = 1;

        for (int i = 1; i < this.octaves; i++)
        {
            point.X *= this.lacunarity;
            point.Y *= this.lacunarity;
            point.Z *= this.lacunarity;

            amp *= this.gain;
            sum -= (1 - Mathf.Abs(this.GetBaseValue(++seedLocal, point))) * amp;
        }

        return sum;
    }
    
    private float GetFractal(Vector3 point)
    {
        switch (this.fractalType)
        {
            case NoiseFractalType.FBM:
            {
                return this.GetFractalFBM(point);
            }

            case NoiseFractalType.Billow:
            {
                return this.GetFractalBillow(point);
            }

            case NoiseFractalType.RigidMulti:
            {
                return this.GetFractalRigidMulti(point);
            }

            default:
            {
                return 0;
            }
        }
    }

    private float GetFractalPerlin(Vector3 point)
    {
        switch (this.fractalType)
        {
            case NoiseFractalType.FBM:
            {
                return GetPerlinFractalFBM(point);
            }

            case NoiseFractalType.Billow:
            {
                return GetPerlinFractalBillow(point);
            }

            case NoiseFractalType.RigidMulti:
            {
                return GetPerlinFractalRigidMulti(point);
            }

            default:
            {
                return 0;
            }
        }
    }

    private float GetFractalSimplex(Vector2 point)
    {
        switch (this.fractalType)
        {
            case NoiseFractalType.FBM:
            {
                return GetSimplexFractalFBM(point);
            }

            case NoiseFractalType.Billow:
            {
                return GetSimplexFractalBillow(point);
            }

            case NoiseFractalType.RigidMulti:
            {
                return GetSimplexFractalRigidMulti(point);
            }

            default:
            {
                return 0;
            }
        }
    }
    private float GetFractalCubic(Vector2 point)
    {
        switch (this.fractalType)
        {
            case NoiseFractalType.FBM:
            {
                return GetCubicFractalFBM(point);
            }

            case NoiseFractalType.Billow:
            {
                return GetCubicFractalBillow(point);
            }

            case NoiseFractalType.RigidMulti:
            {
                return GetCubicFractalRigidMulti(point);
            }

            default:
            {
                return 0;
            }
        }
    }
}