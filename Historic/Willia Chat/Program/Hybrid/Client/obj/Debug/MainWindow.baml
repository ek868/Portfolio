   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   -   5          � Title$  The Client - Willia Chat��5   	   $	��550��6*   $	��600��67   
�������.��5	      �  � RowDefinitions 5   
   �� 5      �� 5      $	��420 �6   �� 5      �� 5      $��2* �6   �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      $
��3.5* �6   �� 5      .+�5   
   o� $��0��6   $��0��6   $��10��6,   .@�5      m� -   5         m�Name$ Themes��5      $��_Themes��6(   .@�5      m� -   5        $ BlackB��5      $��_Black Background��6+    m� IsCheckable . 5      5       �� 5!      m� 5"      -   5        $ BtnQuit��5"      $��_Disconnect��6(   6=   �� 5#      m� 5$      -   5        $ BtnSave��5$      $��_Save��6(   67   5%      �� 5(   
   -   5         ��Name$ BtnSend��5(      
������ $��0q�6;   $��2��6O   $��3��6_   
������$��1q�6�   $��Franklin Gothic Medium)�6�   .��6�   Send5*      �� 5+   
   -   5        $ BtnExtra��5+      
������ $��0q�6<   $��0��6P   $��3��6`   
������$��1q�6�   $��Franklin Gothic Medium)�6�   .��6�   Extra Chars5-      �� 5.   
   -   5         ��Name$ TxtInput��5.      $��1��6$   $��3��64   $��2q�6A   .��6o   .��6�   50      �� 51   
   -	   5         ��Name$ rtxtDisplay��51       �� 
IsReadOnly . $��1��6=   $��1��6M   $��2��6Z   $��5q�6n   
�������. �52      /� .T�53      J� -
   5        	 J�Name$	 P1��53      54      55      �� 56   
   -   5        
 ��Name$
 WhyNot��56      $��4��6$   $��7(�61   © V1.1 Willia Chat™58      �� 5:   
   -   5         ��Name$ LsbConnected��5:   :   $��0��6   $��1��6"   $��5q�6/   5<      5=      5>      