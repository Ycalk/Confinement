using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Confinement.View.Scenes.Cubes.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Confinement.View
{
    internal static class Content
    {
        public const float CubeSize = 2.0f;
        public const float CubeSizeWithOffset = 2.1f;

        #region Fonts

        public static SpriteFont Regular { get; private set; }
        public static SpriteFont GameName { get; private set; }
        public static SpriteFont GameOver { get; private set; }

        #endregion

        #region CommonTextures

        public static Texture2D TranslucentBlack { get; private set; }
        public static Texture2D MainMenuImage { get; private set; }
        public static Texture2D PauseTexture { get; private set; }

        #endregion

        #region CommonButton

        public static Texture2D ButtonRegular { get; private set; }
        public static Texture2D ButtonHover { get; private set; }
        public static Texture2D ButtonClick { get; private set; }

        #endregion

        #region PauseButton
        public static Texture2D PauseButtonRegular { get; private set; }
        public static Texture2D PauseButtonHover { get; private set; }
        public static Texture2D PauseButtonClick { get; private set; }

        #endregion

        #region CrossButton

        public static Texture2D CrossButtonRegular { get; private set; }
        public static Texture2D CrossButtonHover { get; private set; }
        public static Texture2D CrossButtonClick { get; private set; }

        #endregion

        #region InstructionButton

        public static Texture2D InstructionButtonRegular { get; private set; }
        public static Texture2D InstructionButtonHover { get; private set; }
        public static Texture2D InstructionButtonClick { get; private set; }

        #endregion

        #region CubeButton

        public static Texture2D CubeButtonRegular { get; private set; }
        public static Texture2D CubeButtonHover { get; private set; }
        public static Texture2D CubeButtonClick { get; private set; }

        #endregion

        #region EmptyButton

        public static Texture2D EmptyButtonRegular { get; private set; }
        public static Texture2D EmptyButtonHover { get; private set; }
        public static Texture2D EmptyButtonClick { get; private set; }


        #endregion

        #region MainMenuButoon

        public static Texture2D MainMenuButtonRegular { get; private set; }
        public static Texture2D MainMenuButtonHover { get; private set; }
        public static Texture2D MainMenuButtonClick { get; private set; }

        #endregion

        #region RefreshButton

        public static Texture2D RefreshButtonRegular { get; private set; }
        public static Texture2D RefreshButtonHover { get; private set; }
        public static Texture2D RefreshButtonClick { get; private set; }

        #endregion

        #region NextButton

        public static Texture2D NextButtonRegular { get; private set; }
        public static Texture2D NextButtonHover { get; private set; }
        public static Texture2D NextButtonClick { get; private set; }

        #endregion

        #region Cubes
        public static Model RoundedCube { get; private set; }
        public static Texture2D GreyTexture { get; private set; }
        public static Texture2D DarkGreyTexture { get; private set; }
        public static Texture2D GreenTexture { get; private set; }
        public static Texture2D RedTexture { get; private set; }

        #endregion


        private static bool _contentLoaded;

        public static void LoadContent(ContentManager contentManager)
        {
            if (_contentLoaded)
                return;

            Regular = contentManager.Load<SpriteFont>("Fonts/Regular");
            GameName = contentManager.Load<SpriteFont>("Fonts/GameName");
            GameOver = contentManager.Load<SpriteFont>("Fonts/GameOver");

            TranslucentBlack = contentManager.Load<Texture2D>("Buttons/TranslucentBlack");
            MainMenuImage = contentManager.Load<Texture2D>("MainMenuImage");
            PauseTexture = contentManager.Load<Texture2D>("Buttons/PauseTexture");

            ButtonRegular = contentManager.Load<Texture2D>("Buttons/ButtonRegular");
            ButtonHover = contentManager.Load<Texture2D>("Buttons/ButtonHover");
            ButtonClick = contentManager.Load<Texture2D>("Buttons/ButtonClick");

            PauseButtonRegular = contentManager.Load<Texture2D>("Buttons/PauseButtonRegular");
            PauseButtonHover = contentManager.Load<Texture2D>("Buttons/PauseButtonHover");
            PauseButtonClick = contentManager.Load<Texture2D>("Buttons/PauseButtonClick");

            CrossButtonRegular = contentManager.Load<Texture2D>("Buttons/CrossButtonRegular");
            CrossButtonHover = contentManager.Load<Texture2D>("Buttons/CrossButtonHover");
            CrossButtonClick = contentManager.Load<Texture2D>("Buttons/CrossButtonClick");

            InstructionButtonRegular = contentManager.Load<Texture2D>("Buttons/QuestionButtonRegular");
            InstructionButtonHover = contentManager.Load<Texture2D>("Buttons/QuestionButtonHover");
            InstructionButtonClick = contentManager.Load<Texture2D>("Buttons/QuestionButtonClick");

            CubeButtonRegular = contentManager.Load<Texture2D>("Buttons/CubeButtonRegular");
            CubeButtonHover = contentManager.Load<Texture2D>("Buttons/CubeButtonHover");
            CubeButtonClick = contentManager.Load<Texture2D>("Buttons/CubeButtonClick");

            EmptyButtonRegular = contentManager.Load<Texture2D>("Buttons/RoundedButtonRegular");
            EmptyButtonHover = contentManager.Load<Texture2D>("Buttons/RoundedButtonHover");
            EmptyButtonClick = contentManager.Load<Texture2D>("Buttons/RoundedButtonClick");

            MainMenuButtonRegular = contentManager.Load<Texture2D>("Buttons/HomeButtonRegular");
            MainMenuButtonHover = contentManager.Load<Texture2D>("Buttons/HomeButtonHover");
            MainMenuButtonClick = contentManager.Load<Texture2D>("Buttons/HomeButtonClick");

            RefreshButtonRegular = contentManager.Load<Texture2D>("Buttons/RefreshButtonRegular");
            RefreshButtonHover = contentManager.Load<Texture2D>("Buttons/RefreshButtonHover");
            RefreshButtonClick = contentManager.Load<Texture2D>("Buttons/RefreshButtonClick");

            NextButtonRegular = contentManager.Load<Texture2D>("Buttons/NextButtonRegular");
            NextButtonHover = contentManager.Load<Texture2D>("Buttons/NextButtonHover");
            NextButtonClick = contentManager.Load<Texture2D>("Buttons/NextButtonClick");

            RoundedCube = contentManager.Load<Model>("RoundedCube/RoundedCube");
            GreyTexture = contentManager.Load<Texture2D>("RoundedCube/GreyTexture");
            DarkGreyTexture = contentManager.Load<Texture2D>("RoundedCube/DarkGreyTexture");
            GreenTexture = contentManager.Load<Texture2D>("RoundedCube/GreenTexture");
            RedTexture = contentManager.Load<Texture2D>("RoundedCube/RedTexture");
            
            _contentLoaded = true;
        }

    }
}
