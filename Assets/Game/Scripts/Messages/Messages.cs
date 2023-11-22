namespace AltaTestWork
{
    public sealed class Messages
    {
        public sealed class Player
        {
            public class PlayerSizePercentChanged
            {
                public readonly float Limit;
                public readonly float Percent;

                public PlayerSizePercentChanged(float limit, float percent)
                {
                    Limit = limit;
                    Percent = percent;
                }
            }

            public class PlayerFail
            {
            }


            public class PlayerIsNearFinish
            {
            }

            public class PlayerHasEnteredFinish
            {
            }
        }
    }
}