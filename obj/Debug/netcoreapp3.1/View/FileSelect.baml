   M S B A M L   `   `   `  ����  MVVMFileMange    MVVM.View.FileSelect   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5         RedStyle#��[  5        � Title$  
MainWindow��5      $	��450��6   $	��800��6$   .��5	      �  � RowDefinitions 5
      �� 5   
   $	��100 �6   �� 5   
   $��* �6   .+�5      �� $ ��ExFile Manager Application��6   $��Center=�62   $��	20,5,20,0q�6O   �� 5      -   5        $��FileNameTextBox��5   2   $��Center=�5      $��40��6+   $��	30,35,0,0q�67    �� TextWrapping$
 Wrap=�6J   $	��Top=�5      $	��476��6&   ��5      �� *
fileName+5   2   �� 5      -   5         ��Name$ BrowseButton��5      $��Browse a file��6#   $��Right=�6;   $��485,35,10,0q�6X   $	��Top=�5      $	��121��6   $��40��6)   ��67   ��  �� Path$ UpdateCommand�� 5      $��2��6   .+�5   
   �� .��5      �� -   5        $��
TextBlock1��5   r   $
��Left=�6   $��	30,96,0,0q�63   $
��Wrap=�6F   $	��Top=�6Z   ��5      �� *FileContent+5      5      5      5      