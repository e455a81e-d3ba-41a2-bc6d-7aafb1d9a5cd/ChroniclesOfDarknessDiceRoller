﻿using System.Timers;
using Timer = System.Timers.Timer;

namespace ChroniclesOfDarknessDiceRoller.Client.Services
{

    public class ToastService : IDisposable
    {
        public event Action<string, ToastLevel>? OnShow;
        public event Action? OnHide;
        private Timer? Countdown;

        public void ShowToast(string message, ToastLevel level)
        {
            OnShow?.Invoke(message, level);
            StartCountDown();
        }

        private void StartCountDown()
        {
            SetCountdown();
            if (Countdown!.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else
            {
                Countdown.Start();
            }
        }

        private void SetCountdown()
        {
            if (Countdown == null)
            {
                Countdown = new Timer(5000);
                Countdown.Elapsed += HideToast;
                Countdown.AutoReset = false;
            }
        }

        private void HideToast(object? sender, ElapsedEventArgs e)
        {
            OnHide?.Invoke();
        }

        public void Dispose()
        {
            Countdown?.Dispose();
        }
    }
}