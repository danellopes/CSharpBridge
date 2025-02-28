﻿using Autofac;

class BridgeDesignPattern
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            System.Console.WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    public class RasterRendered : IRenderer
    {
        public void RenderCircle(float radius)
        {
            System.Console.WriteLine($"Drawing pixels for circle with radius {radius}");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    static void Main(string[] args)
    {
        // IRenderer renderer = new RasterRendered();
        // var circle = new Circle(renderer, 5);
        // circle.Draw();
        // circle.Resize(2);
        // circle.Draw();

        var cb = new ContainerBuilder();
        cb.RegisterType<VectorRenderer>().As<IRenderer>();
        cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0)));

        using (var c = cb.Build())
        {
            var circle = c.Resolve<Circle>(
                new PositionalParameter(0, 5.0f)
            );

            circle.Draw();
            circle.Resize(2);
            circle.Draw();
        }
    }
}