using System;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class Camera
    {
        private Matrix view;
        private Vector2 position;
        private Matrix transMinus;
        private Matrix transPluss;

        public Camera()
            : this(new Matrix(), new Vector2(0, 0))
        {

        }

        public Camera(Matrix view, Vector2 position)
        {
            this.view = view;
            this.position = position;

            /*
            this.transMinus =
                Matrix.CreateTranslation(new Vector3(position - Renderer.getInstance.getScreenCenter(), 0f));
            this.transPluss =
                Matrix.CreateTranslation(new Vector3(Renderer.getInstance.getScreenCenter() - position, 0f));
            */
        }

        public Matrix getView()
        {
            return view;
        }

        public void setView(Matrix view)
        {
            this.view = view;
        }

        public Vector2 getPosition()
        {
            return position;
        }
    
        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void update(GameTime gameTime)
        {
            bool hasCollided = SceneManager.getInstance.getPlayer1().Properties.getProperty<bool>("HasCollided");
            Vector2 velocity = SceneManager.getInstance.getPlayer1().Properties.getProperty<Vector2>("Velocity");

            if (!hasCollided)
            {
                position += -velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            view = Matrix.CreateTranslation(new Vector3(position - Renderer.getInstance.getScreenCenter(), 0f)) *
                Matrix.CreateTranslation(new Vector3(Renderer.getInstance.getScreenCenter(), 0f));   

            /*
            this.view =
                transMinus
                *
                Matrix.CreateScale(Renderer.getInstance.screenScalingFactor)
                *
                transPluss
                *
                Matrix.CreateTranslation(new Vector3(position - Renderer.getInstance.getScreenCenter(), 0f))
                *
                Matrix.CreateTranslation(new Vector3(Renderer.getInstance.getScreenCenter(), 0f));
                */
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            player1Velocity = player1Velocity / 2;

            position += -player1Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds +
                    -collisionResult.MinimumTranslationVector;
        }
    }
}