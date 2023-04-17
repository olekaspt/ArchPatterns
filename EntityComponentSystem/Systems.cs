

using SampleECS;

class Systems
{

    public static void RunHealthSystem(Registry registry)
    {
        var view = registry.View<SampleECS.Components.LivingCreature.Health>();
        foreach (var entity in view)
        {
            ref SampleECS.Components.LivingCreature.Health health = ref registry.GetComponent<SampleECS.Components.LivingCreature.Health>(entity);
            Console.WriteLine($"Health entity: {entity}, hp: {health.hp}");
        }
    }

    public static void RunVelocitySystem(Registry registry)
    {
        var view = registry.View<SampleECS.Components.Positional.Velocity, SampleECS.Components.Positional.Position>();
        foreach (var entity in view)
        {
            ref SampleECS.Components.Positional.Position pos = ref registry.GetComponent<SampleECS.Components.Positional.Position>(entity);
            ref SampleECS.Components.Positional.Velocity vel = ref registry.GetComponent<SampleECS.Components.Positional.Velocity>(entity);
            pos.X += vel.X;
            pos.Y += vel.Y;
        }
    }

    public static void RunPrinterSystem(Registry registry)
    {
        Console.WriteLine("----- Printer -----");
        var view = registry.View<SampleECS.Components.Positional.Velocity, SampleECS.Components.Positional.Position>();
        foreach (var entity in view)
        {
            var pos = registry.GetComponent<SampleECS.Components.Positional.Position>(entity);
            Console.WriteLine($"Printer entity: {entity}, pos: {pos.X},{pos.Y}");
        }
    }

    internal static void RunRender(Registry registry)
    {
        var view = registry.View<SampleECS.Components.Positional.Velocity, SampleECS.Components.Positional.Position, SampleECS.Components.Displayable.SpriteInfo>();
        foreach (var entity in view)
        {
            var pos = registry.GetComponent<SampleECS.Components.Positional.Position>(entity);
            var sprite = registry.GetComponent<SampleECS.Components.Displayable.SpriteInfo>(entity);
            Console.WriteLine($"Render entity: {entity}, pos: {pos.X},{pos.Y}, color : {sprite.color}");
        }
    }
}