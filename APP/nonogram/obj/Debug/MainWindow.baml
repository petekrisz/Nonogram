   M S B A M L   `   `   `  ����C  ?nonogram, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:nonogram.MVVM.ViewModelnonogram.MVVM.ViewModel  5   	   7 clr-namespace:nonogram.MVVM.Viewnonogram.MVVM.View  5   	   #clr-namespace:nonogramnonogram  5   	       nonogram.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4vm%clr-namespace:nonogram.MVVM.ViewModel     5   	   1view clr-namespace:nonogram.MVVM.View     5   	   (localclr-namespace:nonogram     5   	     � Title$  
MainWindow��5
   	   $	��720��6   $
��1280��6)    � WindowStyle$
 None=�66    � 
ResizeMode$ NoResize=�6I   
������  � AllowsTransparency . $��Center=�6�   $��Center=�6�    � 	Resources 5       �� DataType0   *nonogram.MVVM.ViewModel.HelpTableViewModel&    nonogram.MVVM.View.HelpTableView��%G   (��       5      *, +)�� 6
   "  6   .R�5        5      5       � DataContext 5      +   %nonogram.MVVM.ViewModel.MainViewModel  5   
   .��5      �� ���#1A0D00 �� CornerRadius$ 25��6"   ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��250 �6#   �� 5      	 � RowDefinitions	 5      �� 5      $	��130 �6    �� 5      .+�5       � -   5        
 �Name$
 SideBar��5       $��1��6$    5!      �� 5"      $��2* �6'   �� 5#      $��3* �6'   	 5%      �� 5&      $	��160 �6$   �� 5'      �� 5(      $	��350 �6$   .+�5+      �� $��2��6!   $��
4,10,10,10q�65   .��5,      ��  �� Orientation$ Vertical=�6!   .+�5-      	� -   5         	�Name$ 	NewButton��5-   &   $��	New Image��69   $��20(�6M   $��44��6[   $��0,4,60,0q�6g      
MenuButton#��[  6y   ��6�   �� *ImageListViewCommand+	� 5.      -   5        $ 
HelpButton��5.   &   $��Buy Help��6:   $��20(�6M   $��44��6[   $��0,0,60,0q�6g   #��[  6y   ��6�   �� *BuyHelpViewCommand+	� 5/      -   5        $ 
ExitButton��5/   &   $��  Exit��6:   $��20(�6K   $��44��6Y   $��0,0,60,4q�6e   #��[  6w   6�   50      51      �� 52      $��1��6!   $��2��6.   54      �� 55      $��2��6!   $��0��6.   57      �� 59      -   5         ��Name$ HelpSlot��59   !   $��2��63   $��1��6@   ��6P   �� *CurrentViewHelp+5:      5;      �� 5=      $ 
Horizontal=�6   $��0��62   $��0��6?   .+�5>      �� $��	NONO
GRAM��6   $��Right=�62   $	��Top=�6N   ���#998000$��38(�6{   $��	20,17,0,0q�6�   $��/Fonts/#Kabala)�6�   $��	ExtraBold"�6�   $
��Wrap=�6�   �� 5?      -   5         ��Name$
 logo��5?      $��/Images/logo.png��6&   $��90��6@   $
��Left=�6L   $	��Top=�6g   $��	5,18,0,10q�6�   5@      �� 5A      -   5        $ TitleBar��5A      $��0��6/   $��1��6<   $��10q�6L   ��6X   �� *CurrentViewTitle+�� 5B      $	��Top=�6   $��Right=�60   $	��120��6L   $	��140��6X   $��1��6e    �� RadiusX$ 20��6r    �� RadiusY$ 20��6   $��0,10,0,0q�6�   ��5C      �� 5D      -   5        
avatar�� j� Stretch$
 None=�5D   S    �� ImageSource 61   �� *	AvatarUrl+6S   5F      �� 5H      $��1��6   $��1��6(   $��25q�68   $
��Auto=�5I      $
��Auto=�5J        CustomScrollViewerStyle#��[ 5K      .��5L      �� ��6!   �� *CurrentViewMain+5M      5N      5O      5P      