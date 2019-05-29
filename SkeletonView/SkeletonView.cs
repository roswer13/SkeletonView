using System;
using System.Threading.Tasks;
using SkeletonView.Helps;
using Xamarin.Forms;

namespace SkeletonView
{
    public static class SkeletonView
    {
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.CreateAttached("IsLoading",
                                                                                                   typeof(bool),
                                                                                                   typeof(ContentPage),
                                                                                                   default(bool),
                                                                                                   BindingMode.TwoWay,
                                                                                                   propertyChanged: (bindable, oldValue, newValue) =>
                                                                                                   OnIsDebugChanged(bindable, (bool)oldValue, (bool)newValue));

        public static void SetIsLoading(BindableObject b, bool value)
        {
            b.SetValue(IsLoadingProperty, value);
        }

        public static bool GetIsLoading(BindableObject b)
        {
            return (bool)b.GetValue(IsLoadingProperty);
        }

        static bool IsReplay { get; set; }

        static void OnIsDebugChanged(BindableObject bindable, bool oldValue, bool newValue)
        {
            // Property changed implementation goes here
            if (!bindable.GetType().IsSubclassOf(typeof(Page)))
                return;

            if (newValue)
            {
                IsReplay = true;
                (bindable as Page).Appearing += Page_Appearing;
            }
            else
                IsReplay = false;
        }

        static void Page_Appearing(object sender, EventArgs e)
        {
            if (sender.GetType().IsSubclassOf(typeof(ContentPage)))
                IterateChildren((sender as ContentPage).Content);

            else if (sender is IViewContainer<Page>)
            {
                var tabbedPage = sender as IViewContainer<Page>;

                foreach (var item in tabbedPage.Children)
                {
                    if (item is ContentPage)
                        IterateChildren(((ContentPage)item).Content);
                }
            }
        }

        static void IterateChildren(Element content)
        {
            if (content != null)
            {
                if (content.GetType().IsSubclassOf(typeof(Layout)))
                {
                    foreach (var item in ((Layout)content).Children)
                        IterateChildren(item);
                }
                else if (content.GetType().IsSubclassOf(typeof(View)))
                {
                    if (!(content.GetType() == typeof(Label)))
                        return;

                    Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () => { await SkeletonAnimation(content); });
                    });
                }
            }
        }

        async static Task SkeletonAnimation(Element content)
        {
            await Task.WhenAll(
                         ((Label)content).ColorTo(Color.Transparent, Color.Transparent, c => ((Label)content).TextColor = c, 1000),
                         ((Label)content).ColorTo(Color.DarkGray, Color.White, c => ((Label)content).BackgroundColor = c, 1000));
            await Task.WhenAll(
                 ((Label)content).ColorTo(Color.Transparent, Color.Transparent, c => ((Label)content).TextColor = c, 1000),
                 ((Label)content).ColorTo(Color.White, Color.DarkGray, c => ((Label)content).BackgroundColor = c, 1000));

            if (IsReplay)
                await SkeletonAnimation(content);
            else
            {
                ((Label)content).TextColor = Color.Default;
                ((Label)content).BackgroundColor = Color.Default;
            }
        }
    }
}
;