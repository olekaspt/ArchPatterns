
namespace SampleECS
{
    namespace Components
    {

        namespace Positional
        {
            struct Position
            {
                public float X, Y;
            }

            struct Velocity
            {
                public float X, Y;
            }
        }

        namespace LivingCreature
        {
            struct Health
            {
                public int hp;
            }
        }

        namespace Displayable
        {

            struct SpriteInfo
            {
                public int color;
            }


        }
    }

}