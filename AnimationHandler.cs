
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace On_Screen_Keyboard
{
    public class AnimationHandler : Page
    {
        private const int shrinkModifier = 10;
        private const int animationDuration = 50;
        private Button button;
        private double previousWidth;
        private double previousHeight;
        private Thickness previousMargin;
        public AnimationHandler(Button button)
        {
            this.button = button;
            previousWidth = button.Width;
            previousHeight = button.Height;
            previousMargin = button.Margin;

            NameScope.SetNameScope(this, new NameScope());
            RegisterName(button.Name, button);
        }

        public void ButtonMouseDownAnimation(object sender, RoutedEventArgs e)
        {
            DoubleAnimation widthShrinkAnimation = new DoubleAnimation(button.Width - shrinkModifier, TimeSpan.FromMilliseconds(animationDuration));
            Storyboard.SetTargetName(widthShrinkAnimation, button.Name);
            Storyboard.SetTargetProperty(widthShrinkAnimation, new PropertyPath(Button.WidthProperty));

            Thickness margin = new Thickness();
            margin.Left = shrinkModifier / 2 + button.Margin.Left;
            margin.Top = shrinkModifier / 2 + button.Margin.Top;
            margin.Right = shrinkModifier / 2 + button.Margin.Right;
            margin.Bottom = shrinkModifier / 2 + button.Margin.Bottom;

            ThicknessAnimation marginExpandAnimation = new ThicknessAnimation(margin, TimeSpan.FromMilliseconds(animationDuration));
            Storyboard.SetTargetName(marginExpandAnimation, button.Name);
            Storyboard.SetTargetProperty(marginExpandAnimation, new PropertyPath(Button.MarginProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(widthShrinkAnimation);
            storyboard.Children.Add(marginExpandAnimation);

            if (button.Name.Equals("Btn_ENTER"))    //the height of Btn_ENTER is not Auto so you have to set it manually
            {
                DoubleAnimation heightShrinkAnimation = new DoubleAnimation(button.Height - shrinkModifier, TimeSpan.FromMilliseconds(animationDuration));
                Storyboard.SetTargetName(heightShrinkAnimation, button.Name);
                Storyboard.SetTargetProperty(heightShrinkAnimation, new PropertyPath(Button.HeightProperty));
                storyboard.Children.Add(heightShrinkAnimation);
            }

            storyboard.Begin(this);
        }
        
        public void ButtonMouseUpAnimation(object sender, RoutedEventArgs e)
        {
            DoubleAnimation widthExpandAnimation = new DoubleAnimation(previousWidth, TimeSpan.FromMilliseconds(animationDuration));
            Storyboard.SetTargetName(widthExpandAnimation, button.Name);
            Storyboard.SetTargetProperty(widthExpandAnimation, new PropertyPath(Button.WidthProperty));

            ThicknessAnimation marginShrinkAnimation = new ThicknessAnimation(previousMargin, TimeSpan.FromMilliseconds(animationDuration));
            Storyboard.SetTargetName(marginShrinkAnimation, button.Name);
            Storyboard.SetTargetProperty(marginShrinkAnimation, new PropertyPath(Button.MarginProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(widthExpandAnimation);
            storyboard.Children.Add(marginShrinkAnimation);

            if (button.Name.Equals("Btn_ENTER"))    //the height of Btn_ENTER is not Auto so you have to reset it manually
            {
                DoubleAnimation heightShrinkAnimation = new DoubleAnimation(previousHeight, TimeSpan.FromMilliseconds(animationDuration));
                Storyboard.SetTargetName(heightShrinkAnimation, button.Name);
                Storyboard.SetTargetProperty(heightShrinkAnimation, new PropertyPath(Button.HeightProperty));
                storyboard.Children.Add(heightShrinkAnimation);
            }

            storyboard.Begin(this);
        }

    }
}
