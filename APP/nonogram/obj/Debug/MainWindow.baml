   M S B A M L   `   `   `  ����C  ?nonogram, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:nonogram.MVVM.ViewModelnonogram.MVVM.ViewModel  5   	   7 clr-namespace:nonogram.MVVM.Viewnonogram.MVVM.View  5   	   #clr-namespace:nonogramnonogram  5   	       nonogram.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4vm%clr-namespace:nonogram.MVVM.ViewModel     5   	   1view clr-namespace:nonogram.MVVM.View     5   	   (localclr-namespace:nonogram     5   	   <sys,http://schemas.microsoft.com/winfx/2009/xaml     6.     � Title$  
MainWindow��5
   	   $	��720��6   $
��1280��6)    � WindowStyle$
 None=�66    � 
ResizeMode$ NoResize=�6I   
������  � AllowsTransparency . $��Center=�6�   $��Center=�6�    � 	Resources 5       �� DataType0   *nonogram.MVVM.ViewModel.HelpTableViewModel&    nonogram.MVVM.View.HelpTableView��%G   (��       5      *, +)�� 6
   "  6   .R�5        5      5      .��5      �� ���#1A0D00 �� CornerRadius$ 25��6"   ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��250 �6#   �� 5       � RowDefinitions 5      �� 5      $	��130 �6    �� 5      .+�5      � -   5        	 �Name$	 SideBar��5      $��1��6$    5      �� 5      $��2* �6'   �� 5      $��3* �6'    5       �� 5!      $	��160 �6$   �� 5"      �� 5#      $	��350 �6$   .+�5&      �� $��2��6!   $��
4,10,10,10q�65   .��5'      �� 
 �� Orientation$
 Vertical=�6!   .+�5(      	� -   5         	�Name$ 	NewButton��5(   &   $��	New Image��69   $��20(�6M   $��44��6[   $��0,4,60,0q�6g      
MenuButton#��[  6y   ��6�   �� *ImageListViewCommand+	� 5)      -   5        $ 
HelpButton��5)   &   $��Buy Help��6:   $��20(�6M   $��44��6[   $��0,0,60,0q�6g   #��[  6y   ��6�   �� *BuyHelpViewCommand+	� 5*      -   5        $ 
ExitButton��5*   &   $��  Exit��6:   $��20(�6K   $��44��6Y   $��0,0,60,4q�6e   #��[  6w   6�   5+      5,      �� 5-      $��1��6   $��2��6*   $
 Vertical=�6>   .+�5.      �� -   5         ��Name$ 
UserNameTB��5.       $��30(�5/       $��
ExtraBlack"�50       $��Right=�51       $��0,3,3,0q�52       ���#FFD500$��/Fonts/#Kabala)�53   5   ��5.   4   �� *User.UserName+53   5   �� 54      $
 
Horizontal=�6!   .+�55      �� -   5        $ TokensTB��55   $   $��20(�56   $   $
��Bold"�57   $   $��0,0,10,0q�58   $   $��85��59   $    �� TextAlignment$ Right=�5:   $   $	��Top=�5;   $   ���#998000$��/Fonts/#Kabala)�5<   9   ��55   6   �� *User.Tokens+ �� StringFormat$  {0} ¤��5<   9   �� 5=      -   5        $ ScoreTB��5=   $   $��30(�5>   $   $��	ExtraBold"�5?   $   $��0,0,5,8q�5@   $   $	��150��5A   $   $ Right=�5B   $   ���#FFF7CC$��/Fonts/#Kabala)�5C   9   ��5=   6   �� *
User.Score+5C   9   5E      5F      �� 5G      $��2��6!   $��0��6.   5I      �� 5J      -   5         ��Name$ HelpSlot��5J   !   $��2��63   $��1��6@   ��6P   �� *CurrentViewHelp+5K      5L      �� 5N      $
 
Horizontal=�6   $��0��62   $��0��6?   .+�5O      �� $��	NONO
GRAM��6   $��Right=�62   $	��Top=�6N   ���#998000$��38(�6{   $��	20,17,0,0q�6�   $��/Fonts/#Kabala)�6�   $��	ExtraBold"�6�   $
��Wrap=�6�   �� 5P      -	   5         ��Name$
 logo��5P      $��/Images/logo.png��6&   $��90��6@   $
��Left=�6L   $	��Top=�6g   $��	5,18,0,10q�6�   5Q      �� 5R      -
   5        $ TitleBar��5R      $��0��6/   $��1��6<   $��10q�6L   ��6X   �� *CurrentViewTitle+�� 5S      -   5        $	��Top=�5S      $��Right=�60   $	��120��6L   $	��140��6X   $��1��6e    �� RadiusX$ 20��6r    �� RadiusY$ 20��6   $��0,10,0,0q�6�   ��5T      �� 5U      -   5        
avatar�� j� Stretch$
 None=�5U   S    �� ImageSource 61   �� *	AvatarUrl+6S   5W      �� 5Y      $��1��6   $��1��6(   $��25q�68   $
��Auto=�5Z      $
��Auto=�5[        CustomScrollViewerStyle#��[ 5\      .��5]      �� ��6!   �� *CurrentViewMain+5^      5_      5`      5a      