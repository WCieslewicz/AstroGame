
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CieslewiczAstro2
{
    public class Meteor
    {
        private Texture2D texture;
        private Vector2 position;
        private int nrKlatki;
        private int ileCykli;
        public int score = 0;
        private Vector2 prędkość;
        private Random generujLL;
        private int szerokośćKlatki;
        public Meteor(Texture2D texture, int doLosowania)
        {
            this.texture = texture;
            generujLL = new Random(doLosowania);
            position = new Vector2(generujLL.Next(100, 300), 0);
            nrKlatki = 0;
            ileCykli = 0;
            Startuj(generujLL);
        }
        Vector2 GetPosition()
        {
            return position;
        }
        private void Startuj(Random generujLL)
        {
            position = new Vector2(generujLL.Next(50, 500), -texture.Height);
            prędkość = new Vector2(generujLL.Next(-13, 13) / 2, generujLL.Next(3, 10)) / 2;
        }
        public bool Kolizja(Rakieta gracz)
        {
            Rectangle graczRectangle = new Rectangle((int)gracz.GetPosition().X, (int)gracz.GetPosition().Y, (int)gracz.GetSize().X, (int)gracz.GetSize().X);
            Rectangle wrógRectangle = new Rectangle((int)position.X, (int)position.Y, szerokośćKlatki, texture.Height);
            var result = Rectangle.Intersect(graczRectangle, wrógRectangle);
            Rectangle pociskRectangle = new Rectangle((int)gracz.GetPocisk().X, (int)gracz.GetPocisk().Y, 10, 5);
            var result2 = Rectangle.Intersect(wrógRectangle, pociskRectangle);
            if (!result2.IsEmpty)
            {
                score++;
                Startuj(generujLL);
                gracz.deletePocisk();
            }
            if (result.IsEmpty)
            {
                return false;
            }
            else
                return true;
        }
        public void Update()
        {
            ileCykli++;
            if (ileCykli == 8)
            {
                nrKlatki++;
                ileCykli = 0;
            }
            position += prędkość;
            if (position.Y > 800 || position.X > 480 || position.X < 0)
                Startuj(generujLL);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            szerokośćKlatki = texture.Width / 3;

            Rectangle klatka = new Rectangle(nrKlatki * szerokośćKlatki, 0, szerokośćKlatki, texture.Height);
            Rectangle rectMeteor = new Rectangle((int)position.X, (int)position.Y, klatka.Width, klatka.Height);
            spriteBatch.Draw(texture, rectMeteor, klatka, Color.White);
            if (nrKlatki == 3)
                nrKlatki = 0;
        }
        public int GetScore()
        {
            return score;
        }
    }
}