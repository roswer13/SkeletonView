![https://www.nuget.org/packages/SkeletonView/](/Media/skull.png)

# SkeletonView
It is a portable library designed for Xamarin.Forms that aims to generate **animations** to the elements of the view and can change its binding its state.  It is very useful when loading data when in the view and shows the user that is generating an action.

![](/Media/video.gif)

## How to use it?

The project is up on NuGet at the following URL:

https://www.nuget.org/packages/SkeletonView/

Or run the following command in the Package Manager Console.

    PM> Install-Package SkeletonView -Version 1.0.0

Install this package into your shared project, add the namespace declaration and the new `IsLoading` attached property and set it to `true`!

```
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:skeletonview="clr-namespace:SkeletonView;assembly=SkeletonView"
             x:Class="TestSkeleton.Views.ItemDetailPage" 
             Title="{Binding Title}"
             skeletonview:SkeletonView.IsLoading="{Binding IsLoading}">
             
  ...
             
</ContentPage>
```

## Feedback

Please use [GitHub issues](https://github.com/roswer13/SkeletonView/issues) for questions or comments.

