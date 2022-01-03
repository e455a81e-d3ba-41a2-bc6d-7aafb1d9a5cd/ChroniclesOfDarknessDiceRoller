using ChroniclesOfDarknessDiceRoller.Client.Services;
using Microsoft.AspNetCore.Components;

namespace ChroniclesOfDarknessDiceRoller.Client.Components
{
    public class ToastBase : ComponentBase, IDisposable
    {
        [Inject] private ToastService ToastService { get; set; }

        protected string Heading { get; set; }
        protected string Message { get; set; }
        protected bool IsVisible { get; set; }
        protected string BackgroundCssClass { get; set; }
        protected string IconCssClass { get; set; }

        protected override void OnInitialized()
        {
            ToastService.OnShow += ShowToast;
            ToastService.OnHide += HideToast;
        }

        private void HideToast()
        {
            IsVisible = false;
            StateHasChanged();
        }

        private void ShowToast(string message, ToastLevel level)
        {
            BuildToastSettings(message, level);
            IsVisible = true;
            StateHasChanged();
        }

        private void BuildToastSettings(string message, ToastLevel level)
        {
            switch (level)
            {
                case ToastLevel.Info:
                    BackgroundCssClass = "bg-info";
                    IconCssClass = "info";
                    Heading = "Info";
                    break;

                case ToastLevel.Success:
                    BackgroundCssClass = "bg-success";
                    IconCssClass = "check";
                    Heading = "Success";
                    break;

                case ToastLevel.Warning:
                    BackgroundCssClass = "bg-warning";
                    IconCssClass = "warning";
                    Heading = "Warning";
                    break;

                case ToastLevel.Error:
                    BackgroundCssClass = "bg-danger";
                    IconCssClass = "x";
                    Heading = "Error";
                    break;
            }

            Message = message;
        }

        public void Dispose()
        {
            ToastService.OnShow -= ShowToast;
        }
    }
}