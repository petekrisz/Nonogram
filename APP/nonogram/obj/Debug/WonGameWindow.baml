   M S B A M L   `   `   `  ����C  ?nonogram, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:nonogramnonogram  5   	       nonogram.WonGameWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:nonogram     5   	     � Title$  WonGameWindow��5   	   $	��330��6    � SizeToContent$ Width=�6-      ExitSelectorWindowStyle#��[  5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�6   
������  � AllowsTransparency .  � 	Resources 5        GrowShrinkAnimation g� AutoReverse g� RepeatBehavior	 �� 
TargetName
 �� TargetProperty W� From	 W� To g� Duration��%8  &	       �� 5   
    . $ Forever��6D   .��5      W� $	 	fireworks��6   $
 RenderTransform.ScaleX�5      $	 1.0X�5      $
 0.75X�6)   $ 0:0:1D�63   W� 5      $	 	fireworks��6   $
 RenderTransform.ScaleY�5      $	 1.0X�5      $
 0.75X�6)   $ 0:0:1D�63   5      5      .��5      �� ���#1A0D00 �� CornerRadius$ 10��6"   
��� ���$��3q�6L   ._�5   
   � .+�5      ��  �� Orientation$ Vertical=�6   .+�5      �� $$��Congratulations! You have won!��6    �� TextAlignment$ Center=�5      $	��Top=�5      $��Center=�5      $��18(�5      $
��Bold"�6*   $��0,20,0,5q�6<   �� 5       $��70��6   $��70��6%   
������ $��Center=�6J   $��Center=�6e   ._�5!      �� -   5        $��	fireworks��5!      $
��Fill=�6-   z�5"      �� 5#       �� ScaleX$ 1Q�6-    �� ScaleY$ 1Q�68    �� CenterX$	 0.5Q�6C    �� CenterY$	 0.5Q�6Q   5%      5&      �� 5'      $��Your Prize:��6   $ Center=�5(      $	��Top=�5)      $��Center=�5*      $��18(�5+      $
��Bold"�6*   $��0,5,0,20q�6<   �� 5,      -   5         ��Name$ Prizes��5,      $ 
Horizontal=�6.   $��Center=�5-      $��Bottom=�5.      5/      �� 51      -   5         ��Name$ OK��51      $��OK��6%   $��70��62   $��30��6=     ReddishButtonStyle#��[ 52      53      54      55      56      57      