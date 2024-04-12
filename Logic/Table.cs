﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Data;

namespace Logic
{
    public class Table : LogicAPI, IObserver<DataAPI>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly List<DataAPI> _balls;
        private IDisposable _subscriptionToken;

        public Table(int width, int height)
        {
            this._width = width;
            this._height = height;
            this._balls = new List<DataAPI>();
        }

        public override int Width
        {
            get => _width;
        }

        public override int Height
        {
            get => _height;
        }

        public override List<DataAPI> Balls
        {
            get => _balls;
        }

        public override void CreateBalls(int number, int radius)
        {
            for (int i = 0; i < number; i++)
            {
                var rand = new Random();
                float x = rand.Next(0 + radius, _width - radius);
                float y = rand.Next(0 + radius, _height - radius);
                DataAPI ball = DataAPI.Instance(x, y, radius);
                _balls.Add(ball);
                this.Subscribe(ball);
            }
        }

        //public override void Start(double velocity)
        //{
        //    var rand = new Random();
        //    foreach (var ball in _balls)
        //    {
        //        float newX = rand.Next(0 + ball.Radius, _width - ball.Radius);
        //        float newY = rand.Next(0 + ball.Radius, _height - ball.Radius);
        //        Thread thread = new Thread(() => { ball.Move(newX, newY, velocity); });
        //        thread.Start();
        //    }
        //}

        public override void Start(float velocity)
        {
            var rand = new Random();
            foreach (var ball in _balls)
            {
                float newX = rand.Next(0 + ball.Radius, _width - ball.Radius);
                float newY = rand.Next(0 + ball.Radius, _height - ball.Radius);
                Thread thread = new Thread(() => { ball.Move(velocity); });
                thread.Start();
            }
        }

        public override List<List<float>> GetBallPositions()
        {
            List<List<float>> positions = new List<List<float>>();
            for (int i = 0; i < _balls.Count; i++)
            {
                List<float> position = new List<float>();
                position.Add(_balls[i].X);
                position.Add(_balls[i].Y);
            }
            return positions;
        }

        public override void ResetTable()
        {
            _balls.Clear();
        }

        public void Subscribe(IObservable<DataAPI> provider)
        {
            if (provider != null)
            {
                _subscriptionToken = provider.Subscribe(this);
            }
        }

        public void Unsubscribe()
        {
            if (_subscriptionToken != null)
            {
                _subscriptionToken.Dispose();
            }
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DataAPI value)
        {
            //Console.WriteLine($"Ball moved to {value.X}, {value.Y}");
            DetectWallCollistions();
        }

        private void DetectWallCollistions()
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_balls[i].X + _balls[i].Radius > _width || _balls[i].X - _balls[i].Radius < 0 || _balls[i].Y + _balls[i].Radius > _height || _balls[i].Y - _balls[i].Radius < 0)
                {
                    WallCollistion(i);
                }
            }
        }

        private void WallCollistion(int index)
        {

            DataAPI ball = _balls[index];

            bool hitTopOrBottom = false;

            if (ball.Y - ball.Radius < 0 || ball.Y + ball.Radius > _height)
            {
                hitTopOrBottom = true;
            }

            if (hitTopOrBottom)
            {
                ball.VelocityY *= -1;
                ball.Y = Math.Clamp(ball.Y, ball.Radius, _height - ball.Radius);
            }
            else
            {
                ball.VelocityX *= -1;
                ball.X = Math.Clamp(ball.X, ball.Radius, _width - ball.Radius);
            }

            float newX = ball.X + ball.VelocityX * 0.01f;
            float newY = ball.Y + ball.VelocityY * 0.01f;

            ball.X = newX;
            ball.Y = newY;
        }
    }
}
