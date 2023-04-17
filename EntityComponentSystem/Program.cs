// See https://aka.ms/new-console-template for more information
using SampleECS.Components.LivingCreature;
using SampleECS.Components.Positional;


using Microsoft.Win32;
using System;
using SampleECS.Components.Displayable;

// This code was inspired from https://gist.github.com/prime31/99c66a4aeb4fc0e75173d5ea80f75a97


class Program
{
    public static void Main(string[] args)
    {
        var registry = new Registry(100);

        for (var i = 0; i < 5; i++)
        {
            var entity = registry.Create();
            registry.AddComponent<Position>(entity, new Position { X = i * 10, Y = i * 10 });
            registry.AddComponent<Velocity>(entity, new Velocity { X = 2 + i, Y = 2 + i });
            if(i % 2 == 0)
            {
                registry.AddComponent<Health>(entity, new Health { hp = 5 });
            }
            if(i == 2 || i % 3 == 0)
            {
                registry.AddComponent<SpriteInfo>(entity, new SpriteInfo { color = 34 });
            }
        }

        // Normally this would be a while loop for the game, just doing a couple of iterations
        for (int i = 0; i < 2; i++)
        {
            //Hurt entity 2
            ref SampleECS.Components.LivingCreature.Health health = ref registry.GetComponent<SampleECS.Components.LivingCreature.Health>(2);
            health.hp = health.hp - 1;
            Systems.RunVelocitySystem(registry);
            Systems.RunPrinterSystem(registry);
            Systems.RunRender(registry);
            Systems.RunHealthSystem(registry);



        }
    }


}