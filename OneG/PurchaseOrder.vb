﻿Imports System.DirectoryServices.ActiveDirectory
Imports IBM.Data.DB2

Public Class PurchaseOrder
    Private conn As DB2Connection
    Dim po As Integer
    Dim sidebar As String = "Close"

    Sub clear()
        lblWelcomeBar.Visible = False
        dtpSideBar.Visible = False
        OrderBtn.Visible = False
        PayBtn.Visible = False
        MainBtn.Visible = False
        AccBtn.Visible = False
        PurcBtn.Visible = False
        LogoutBtn.Visible = False
    End Sub


    Sub setname()
        lblWelcomeBar.Visible = True
        dtpSideBar.Visible = True
        OrderBtn.Visible = True
        PayBtn.Visible = True
        MainBtn.Visible = True
        AccBtn.Visible = True
        PurcBtn.Visible = True
        LogoutBtn.Visible = True
    End Sub


    Private Sub PurchaseOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn = New DB2Connection("server=localhost;database=oneg;" + "uid=db2admin;password=db2admin;")
        conn.Open()

        Try
            Dim chkBoxCol As New DataGridViewCheckBoxColumn
            dgvPO.Columns.Add(chkBoxCol)
            dgvPO.Columns(0).Width = 20

            With Me.dgvPO 'PO to be ordered
                .ColumnCount = 11
                .Columns(1).Name = "PURCHASE ORDER NUMBER"
                .Columns(2).Name = "PROCESSED BY"
                .Columns(3).Name = "SUPPLIER"
                .Columns(4).Name = "INGREDIENT"
                .Columns(5).Name = "QTY"
                .Columns(6).Name = "UNIT"
                .Columns(7).Name = "PURCHASE PRICE"
                .Columns(8).Name = "SUBTOTAL"
                .Columns(9).Name = "DELIVERY STATUS"
                .Columns(10).Name = "DATE ORDERED"
            End With

            With Me.dgvSelect
                .ColumnCount = 4
                .Columns(0).Name = "ID"
                .Columns(1).Name = "INGREDIENT"
                .Columns(2).Name = "IN STOCK"
                .Columns(3).Name = "UNIT"
            End With
            Me.lblWelcomeBar.Text = "WELCOME, " + Login.name.ToString + "!"
            Call RefreshorderDataGrid()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RefreshorderDataGrid()
        Try

            Dim cmd As DB2Command
            Dim rdr As DB2DataReader

            Dim rows As String()

            cmd = New DB2Command("select * from INGREDIENTS", conn)
            rdr = cmd.ExecuteReader

            Me.dgvSelect.Rows.Clear()
            While rdr.Read
                rows = New String() {rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3)}
                Me.dgvSelect.Rows.Add(rows)
            End While

            dgvPO.Rows.Clear()


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub MenuBarBtn_Click(sender As Object, e As EventArgs) Handles MenuBarBtn.Click
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If sidebar = "Open" Then
            leftSideBar.Width -= 65
            If leftSideBar.Width <= 65 Then
                clear()
                sidebar = "Close"
                Timer1.Stop()
            End If
        Else
            leftSideBar.Width += 65
            If leftSideBar.Width >= 200 Then
                setname()
                sidebar = "Open"
                Timer1.Stop()
            End If


        End If
    End Sub


    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles closebtn.Click
        Me.Close()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged

        Dim strsearchkey As String
        Dim cmdsearch As DB2Command
        Dim rdrsearch As DB2DataReader
        Dim row As String()

        If SEARCHPO.Text = "Search Ingredients" Then
            Try
                strsearchkey = Me.txtsearch.Text + "%"
                cmdsearch = New DB2Command("select * from ingredients where ingname like '" & strsearchkey & "'", conn)
                rdrsearch = cmdsearch.ExecuteReader
                Me.dgvSelect.Rows.Clear()
                While rdrsearch.Read
                    row = New String() {rdrsearch.GetString(0), rdrsearch.GetString(1), rdrsearch.GetString(2), rdrsearch.GetString(3)}

                    Me.dgvSelect.Rows.Add(row)
                End While
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            Try
                strsearchkey = Me.txtsearch.Text + "%"
                cmdsearch = New DB2Command("select * from SUPPLIER where SUPNAME like '" & strsearchkey & "'", conn)
                rdrsearch = cmdsearch.ExecuteReader
                Me.dgvSelect.Rows.Clear()
                While rdrsearch.Read
                    row = New String() {rdrsearch.GetString(0), rdrsearch.GetString(1), rdrsearch.GetString(2)}

                    Me.dgvSelect.Rows.Add(row)
                End While
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ingbtn.Click
        SEARCHPO.Text = "Search Ingredients"
        With Me.dgvSelect
            .ColumnCount = 4
            .Columns(0).Name = "ID"
            .Columns(1).Name = "INGREDIENT"
            .Columns(2).Name = "IN STOCK"
            .Columns(3).Name = "UNIT"
        End With
        Dim cmd As DB2Command
        Dim rdr As DB2DataReader
        ingbtn.BackColor = Color.Gray
        supbtn.BackColor = Color.FromArgb(106, 112, 124)
        Dim rows As String()

        cmd = New DB2Command("select * from INGREDIENTS", conn)
        rdr = cmd.ExecuteReader

        Me.dgvSelect.Rows.Clear()
        While rdr.Read
            rows = New String() {rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3)}
            Me.dgvSelect.Rows.Add(rows)
        End While

    End Sub

    Private Sub supbtn_Click(sender As Object, e As EventArgs) Handles supbtn.Click
        SEARCHPO.Text = "Search Supplier"
        With Me.dgvSelect
            .ColumnCount = 3
            .Columns(0).Name = "ID"
            .Columns(1).Name = "SUPPLIER NAME"
            .Columns(2).Name = "CONTANCT NO."
        End With
        supbtn.BackColor = Color.Gray
        ingbtn.BackColor = Color.FromArgb(106, 112, 124)
        Dim cmd As DB2Command
        Dim rdr As DB2DataReader

        Dim rows As String()

        cmd = New DB2Command("select * from SUPPLIER", conn)
        rdr = cmd.ExecuteReader

        Me.dgvSelect.Rows.Clear()
        While rdr.Read
            rows = New String() {rdr.GetString(0), rdr.GetString(1), rdr.GetString(2)}
            Me.dgvSelect.Rows.Add(rows)
        End While

    End Sub

    Private Sub dgvSelect_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSelect.CellContentClick

    End Sub

    Private Sub dgvSelect_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvSelect.MouseUp

        Dim cmd As DB2Command
        Dim rdr As DB2DataReader
        Dim i As Integer = 0
        Dim poStr As String
        Dim count As Integer = 0
        Dim similar As Boolean = False
        Dim po As Integer = 0
        If SEARCHPO.Text = "Search Ingredients" Then
            Try

                While dgvPO.Rows(i).Cells(1).Value IsNot Nothing
                    If dgvPO.Rows(i).Cells(4).Value = dgvSelect.CurrentRow.Cells(0).Value Then
                        similar = True
                    End If
                    i += 1
                End While



                'get the last po_no
                Me.dgvPO.Rows.Add()
                cmd = New DB2Command("select po_no from purchases order by po_no asc", conn)
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    While rdr.Read
                        poStr = rdr.GetString(0)
                        Integer.TryParse(poStr, po)
                    End While
                Else
                    po = 10000 'as start
                End If


                'increment last po_no 
                If po <> 10000 Then
                    po += 1
                End If


                'condition where if above cells are not empty then add 1 to current po
                While count < i
                    'if has similar purchase order existing in database then move on
                    cmd = New DB2Command("select po_no from purchases where po_no='" & Me.dgvPO.Rows(count).Cells(1).Value & "'", conn)
                    rdr = cmd.ExecuteReader
                    If rdr.HasRows Then
                        count += 1
                    Else
                        'current row is equal to po +1
                        po += 1
                        count += 1
                    End If
                End While

                Me.dgvPO.Rows(i).Cells(1).Value = po
                Me.dgvPO.Rows(i).Cells(4).Value = Me.dgvSelect.CurrentRow.Cells(0).Value
                Me.dgvPO.Rows(i).Cells(2).Value = Login.ACCID.ToString
                Me.dgvPO.Rows(i).Cells(6).Value = Me.dgvSelect.CurrentRow.Cells(3).Value
                Me.dgvPO.Rows(i).Cells(8).Value = "0"
                Me.dgvPO.Rows(i).Cells(10).Value = Me.dtpSideBar.Value.ToShortDateString
                Me.dgvPO.Rows(i).Cells(9).Value = "NOT DELIVERED"

                If similar = True Then
                    MsgBox("Reminder: Ingredient already ordered in the list")
                    dgvPO.Rows.RemoveAt(i)
                End If

            Catch ex As Exception
                MsgBox(ex.ToString)
                dgvPO.ClearSelection()
            End Try
        Else
            Try

                Dim j As Integer = 0

                'if supplier and ingredient is filled increment count
                While dgvPO.Rows(j).Cells(3).Value IsNot Nothing And dgvPO.Rows(j).Cells(4).Value IsNot Nothing
                    j += 1
                End While

                'if ingredient is filled  and supplier empty then add supplier to same row
                If dgvPO.Rows(j).Cells(4).Value IsNot Nothing And dgvPO.Rows(j).Cells(3).Value Is Nothing Then
                    Me.dgvPO.Rows(j).Cells(3).Value = Me.dgvSelect.CurrentRow.Cells(0).Value

                ElseIf j = 0 Then
                    MsgBox("Select Ingredient to assign supplier ID")

                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                dgvPO.ClearSelection()
            End Try
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim count As Integer = 0
        Dim k As Integer = 0
        Dim cmd As DB2Command
        Dim rdr As DB2DataReader
        Dim postr As String
        Dim j As Integer = 0
        Dim none As Boolean = True


        Try
            k = dgvPO.Rows.Count - 1


            While count < k

                If dgvPO.Rows(count).Cells(0).Value = True Then
                    dgvPO.Rows(count).Cells(0).Value = False
                    dgvPO.Rows.RemoveAt(count)
                    k -= 1
                    none = False
                Else
                    count += 1
                End If
            End While

            If none = True Then
                dgvPO.Rows.RemoveAt(k - 1)
            End If

            cmd = New DB2Command("select po_no from purchases order by po_no asc", conn)
                rdr = cmd.ExecuteReader
                If rdr.HasRows Then
                    While rdr.Read
                        postr = rdr.GetString(0)
                        Integer.TryParse(postr, po)

                End While
                Else
                    po = 10000 'as start
                End If

                'increment last po_no 
                po += 1

            k = 0
            While Me.dgvPO.Rows(k).Cells(1).Value IsNot Nothing
                k += 1
            End While


            While j < k
                    'if has similar purchase order existing inside db then move on and dont change po_no 
                    cmd = New DB2Command("select po_no from purchases where po_no='" & Me.dgvPO.Rows(j).Cells(1).Value & "'", conn)
                    rdr = cmd.ExecuteReader
                    If rdr.HasRows Then
                        j += 1
                    Else
                        ' first row is assigned the value equal to po 
                        If j = 0 Then
                            dgvPO.Rows(j).Cells(1).Value = po
                            po += 1
                            j += 1

                            'check if row-1 has value similar to current po if none then assign that value to current row
                        ElseIf Me.dgvPO.Rows(j - 1).Cells(1).Value <> po Then
                            dgvPO.Rows(j).Cells(1).Value = po
                            j += 1
                        Else
                            'current row is equal to po +1
                            po += 1
                            dgvPO.Rows(j).Cells(1).Value = po
                            j += 1
                        End If
                    End If
                End While

        Catch ex As Exception
            MsgBox(ex.ToString)


        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles SaveBtn.Click
        Dim cmdInsert As DB2Command
        Dim i As Integer = 0 'as count of row that has po_no assigned
        Dim count As Integer = 0
        Dim cmdupdate2 As DB2Command
        Dim rdrInsert As DB2DataReader

        If Login.role.Equals("CASHIER") Then
            MsgBox("Error: User Acount Not Authorized")
        Else
            Try
                While Me.dgvPO.Rows(i).Cells(1).Value IsNot Nothing
                    i += 1
                End While

                While count < i

                    cmdInsert = New DB2Command("select po_no from purchases where po_no ='" & Me.dgvPO.Rows(count).Cells(1).Value & "'", conn)
                    rdrInsert = cmdInsert.ExecuteReader
                    If rdrInsert.HasRows Then
                        cmdInsert = New DB2Command("update purchases set qtypur = @qty,ing_id = @ing,supid =@supid,empid=@emp ,pricepur=@price,subtotal=@sub,delvstat=@delv,purunit=@unit where po_no= '" & Me.dgvPO.Rows(count).Cells(1).Value & "'", conn)
                        cmdInsert.Parameters.Add("@ing", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(4).Value
                        cmdInsert.Parameters.Add("@supid", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(3).Value
                        cmdInsert.Parameters.Add("@qty", DB2Type.Double).Value = Me.dgvPO.Rows(count).Cells(5).Value
                        cmdInsert.Parameters.Add("@unit", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(6).Value
                        cmdInsert.Parameters.Add("@price", DB2Type.Double).Value = Me.dgvPO.Rows(count).Cells(7).Value
                        cmdInsert.Parameters.Add("@sub", DB2Type.Double).Value = Me.dgvPO.Rows(count).Cells(8).Value
                        cmdInsert.Parameters.Add("@delv", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(9).Value
                        cmdInsert.Parameters.Add("@emp", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(1).Value
                        cmdInsert.ExecuteNonQuery()


                    Else
                        cmdInsert = New DB2Command("insert into purchases(po_no,empid,ing_id,supid,qtypur,pricepur,subtotal,delvstat,purdate,purunit) values(@po,@emp,@ing,@supid,@qty,@price,@sub,@delv,@date,@unit)", conn)
                        cmdInsert.Parameters.Add("@po", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(1).Value
                        cmdInsert.Parameters.Add("@emp", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(2).Value
                        cmdInsert.Parameters.Add("@ing", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(4).Value
                        cmdInsert.Parameters.Add("@supid", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(3).Value
                        cmdInsert.Parameters.Add("@qty", DB2Type.Double).Value = Me.dgvPO.Rows(count).Cells(5).Value
                        cmdInsert.Parameters.Add("@unit", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(6).Value
                        cmdInsert.Parameters.Add("@price", DB2Type.Double).Value = Me.dgvPO.Rows(count).Cells(7).Value
                        cmdInsert.Parameters.Add("@sub", DB2Type.Double).Value = Me.dgvPO.Rows(count).Cells(8).Value
                        cmdInsert.Parameters.Add("@delv", DB2Type.VarChar).Value = Me.dgvPO.Rows(count).Cells(9).Value
                        cmdInsert.Parameters.Add("@date", DB2Type.Date).Value = Me.dgvPO.Rows(count).Cells(10).Value
                        cmdInsert.ExecuteNonQuery()

                    End If


                    Dim stat As String = Me.dgvPO.Rows(count).Cells(9).Value
                    Dim unit As String

                    If stat = "DELIVERED" Or stat = "NOT DELIVERED" Then
                        Dim Cmdunit As DB2Command
                        Dim rdrunit As DB2DataReader

                        Cmdunit = New DB2Command("select ingunit from ingredients where ing_ID= '" & Me.dgvPO.Rows(count).Cells(4).Value & "'", conn)
                        rdrunit = Cmdunit.ExecuteReader
                        rdrunit.Read()

                        unit = rdrunit.GetString(0)

                        If unit = "KILOGRAMS" And Me.dgvPO.Rows(count).Cells(6).Value = "KILOGRAMS" Then
                            cmdupdate2 = New DB2Command("update ingredients set ingqty = ingqty +@sqty where ing_id= '" & Me.dgvPO.Rows(count).Cells(4).Value & "'", conn)
                            cmdupdate2.Parameters.Add("@sqty", DB2Type.Decimal).Value = Me.dgvPO.Rows(count).Cells(5).Value
                            cmdupdate2.ExecuteNonQuery()

                        ElseIf unit = "KILOGRAMS" And Me.dgvPO.Rows(count).Cells(6).Value = "GRAMS" Then
                            cmdupdate2 = New DB2Command("update ingredients set ingqty = ingqty +@sqty where ing_id= '" & Me.dgvPO.Rows(count).Cells(4).Value & "'", conn)
                            cmdupdate2.Parameters.Add("@sqty", DB2Type.Decimal).Value = Me.dgvPO.Rows(count).Cells(5).Value / 1000
                            cmdupdate2.ExecuteNonQuery()


                        ElseIf unit = "GRAMS" And Me.dgvPO.Rows(count).Cells(6).Value = "KILOGRAMS" Then
                            cmdupdate2 = New DB2Command("update ingredients set ingqty = ingqty +@sqty where ing_id= '" & Me.dgvPO.Rows(count).Cells(4).Value & "'", conn)
                            cmdupdate2.Parameters.Add("@sqty", DB2Type.Decimal).Value = Me.dgvPO.Rows(count).Cells(5).Value * 1000
                            cmdupdate2.ExecuteNonQuery()

                        ElseIf unit = "Grams" And Me.dgvPO.Rows(count).Cells(6).Value = "Grams" Then
                            cmdupdate2 = New DB2Command("update ingredients set ingqty = ingqty +@sqty where ing_no= '" & Me.dgvPO.Rows(count).Cells(4).Value & "'", conn)
                            cmdupdate2.Parameters.Add("@sqty", DB2Type.Decimal).Value = Me.dgvPO.Rows(count).Cells(5).Value
                            cmdupdate2.ExecuteNonQuery()
                        Else
                            cmdupdate2 = New DB2Command("update ingredients set ingqty = ingqty +@sqty where ing_no= '" & Me.dgvPO.Rows(count).Cells(4).Value & "'", conn)
                            cmdupdate2.Parameters.Add("@sqty", DB2Type.Decimal).Value = Me.dgvPO.Rows(count).Cells(5).Value
                            cmdupdate2.ExecuteNonQuery()
                        End If
                    End If

                    count += 1
                End While

                MsgBox("Purchase Order Saved Succesfully")

            Catch ex As Exception
                MsgBox("Tables with missing information is/are not recorded")

            End Try
        End If
        dgvPO.Rows.Clear()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim StrDelete As String
        Dim CmdDelete As DB2Command
        Dim i As Integer = 0
        Dim k As Integer = 0

        While Me.dgvPO.Rows(k).Cells(1).Value IsNot Nothing
            k += 1
        End While

        While i < k
            Try
                StrDelete = "delete from purchases where po_no= '" & Me.dgvPO.Rows(i).Cells(1).Value & "'"
                CmdDelete = New DB2Command(StrDelete, conn)
                CmdDelete.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            i += 1
        End While
        MsgBox("Purchase Order on the List have been canceled")
        dgvPO.Rows.Clear()
    End Sub
End Class