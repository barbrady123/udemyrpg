using UnityEngine;

public static class Global
{
    public static class Colors
    {
        public static readonly Color PlayerName = new Color(0.4f, 0.4f, 0.9f);

        public static readonly Color NPCName = new Color(0.9f, 0.4f, 0.4f);

        public static readonly Color Default = Color.white;
    }

    public static class Labels
    {
        public const string PlayerTag = "Player";

        public const string PlayerChat = "player";

        public const string NPCChat = "npc";

        public const string NoChat = "--";

        public const string DefaultCharacterName = "Player";

        public const string DefaultNPCDisplay = "???";

        public const string DefaultWeaponDisplay = "None";

        public const string DefaultArmorDisplay = "None";
    }

    public static class Inputs
    {
        public const string AxisHorizontal = "Horizontal";

        public const string AxisVertical = "Vertical";

        public const string Fire1 = "Fire1";

        public const string Fire2 = "Fire2";
    }

    public static class Physics
    {
        public const float RunSpeedModifier = 2.0f;
    }

    public static class Scenes
    {
        public const string Init = "Init";

        public const string MainMenu = "MainMenu";

        public const string GameOver = "GameOver";
    }

    public static class Music
    {
        public const int Victory = 0;

        public const int Dungeon = 1;

        public const int Title = 2;

        public const int Town = 3;

        public const int Country = 4;

        public const int Boss = 5;

        public const int Battle = 6;
    }
}
