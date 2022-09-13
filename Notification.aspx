<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notification.aspx.cs" Inherits="Notification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server" style="background-color:antiquewhite;">
        <Center>
            <h1>Send Notification to multiple Device</h1>
            <table style="width:100%;text-align:center;">
              
                 <tr>
                      <td>
                        <asp:Label ID="Label3" runat="server" Text="Title"></asp:Label><br />  
                        <asp:TextBox ID="txttitle" runat="server" Height="80px" Width="274px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                      <tr>
                      <td>
                      </td>
                    </tr>
                     <tr>
                          <tr>
                      <td>
                          </td>
                    </tr>
                      <td>
                        <asp:Label ID="Label2" runat="server" Text="Message"></asp:Label> <br />
                        <asp:TextBox ID="txtMessage" runat="server" Height="80px" Width="274px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                      <td>
                          </td>
                    </tr>
                      <tr>
                      <td>
                          </td>
                    </tr>

                   <tr>
                      <td>
                        <asp:Label ID="Label1" runat="server" Text="Image URL"></asp:Label><br />  
                        <asp:TextBox ID="txtimgurl" runat="server" Height="80px" Width="274px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                          <tr>
                      <td>
                          </td>
                    </tr>
                      <tr>
                      <td>
                          </td>
                    </tr>

                   <tr>
                      <td>
                        <asp:Label ID="Label4" runat="server" Text="ICON URL"></asp:Label><br />  
                        <asp:TextBox ID="txticonurl" runat="server" Height="80px" Width="274px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                          <tr>
                      <td>
                          </td>
                    </tr>
                      <tr>
                      <td>
                          </td>
                    </tr>


                <tr>
                    <td>
                        <asp:Button style="color:blue" ID="btnsendnotification" runat="server" OnClick="btnsendnotification_Click" Text="Send Notification" Width="228px" />
                       <br />
                        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

            </table>
    </Center>
    </form>
</body>
</html>
