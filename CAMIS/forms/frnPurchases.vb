﻿Imports MySql.Data

Public Class frmPurchases
    Private poid As String
    Private vItemid As String

    Public Sub New()
        InitializeComponent()
        'poid = getID("")
        lblEmpID.Text = vEmp
        SqlRefresh = "SELECT company FROM  `supplier` "
        itemAutoComplete("Supplier", txtSupplier)
        SqlRefresh = "select barcode from items group by itemid"
        itemAutoComplete("AutoDescription", txtBarcode)
        SqlRefresh = "select 
                    l.polistid,
                    i.barcode,
                    i.description,
                    concat(i.unitvalue,' ',i.unittype)Unit,
                    l.cost,
                    l.quantity

                    from polist l left join items i
                    on l.itemid=i.itemid
                    where poid =1"
        SqlReFill("polist", ListView1)
        'get current po
        Dim poid As String = getIDFunction("select ifnull(max(poid),1) from po", "purchaseorder", Nothing)
        lblPONum.Text = poid
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        StatusSet = "New"
        Dim sNew As New frmAddUpdateCategory()
        sNew.ShowDialog()
        sNew = Nothing
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If ListView1.SelectedItems.Count = 0 Then
            MessageBox.Show("Select item first", "Selection not identified", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        Else
            StatusSet = "Update"
            Dim update As New frmAddUpdateCategory()
            update.txtCategoryNo.Text = ListView1.FocusedItem.Text
            update.ShowDialog()
        End If
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Me.Close()
    End Sub

    Private Sub frmPurchases_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        If lblSupplierID.Text = vbNullString Then
            openFull(frmSupplier)
        Else
            sqL = "SELECT  `Supplierid` FROM  `supplier` WHERE Company LIKE  @0"
            If getIDFunction(sqL, "SupplierID", {txtSupplier.Text}) = 0 Then    'is supplier is still not present repeat txtsupplier_leave
                txtSupplier_Leave(btnAddItem, e)
            Else

                'MessageBox.Show(vItemid)
                itemNew("POList", {"POID", "ItemID", "Quantity", "Cost"}, {lblPONum, lblItemID, txtQuantity, txtCost})
                MessageBox.Show("Success")
            End If

        End If

    End Sub

    Private Sub txtSupplier_Leave(sender As Object, e As EventArgs) Handles txtSupplier.Leave
        'LOST FOCUS WE WILL GET SUPPLIERID
        Try
            'MessageBox.Show(txtSupplier.Text)
            sqL = "SELECT  `Supplierid` FROM  `supplier` WHERE Company LIKE  @0"
            msgShow = False
            lblSupplierID.Text = getIDFunction(sqL, "SupplierID", {txtSupplier.Text})
        Catch ex As Exception
            MessageBox.Show("error")
        End Try

    End Sub


    Private Sub txtBarcode_Leave(sender As Object, e As EventArgs) Handles txtBarcode.Leave
        Try
            'MessageBox.Show(txtSupplier.Text)
            sqL = "SELECT  `itemid` FROM  `items` WHERE barcode LIKE  @0"
            msgShow = False
            vItemid = getIDFunction(sqL, "items", {txtBarcode.Text})
            lblItemID.Text = vItemid
        Catch ex As Exception
            MessageBox.Show("error")
        End Try
    End Sub

    Private Sub lblItemID_TextChanged(sender As Object, e As EventArgs) Handles lblItemID.TextChanged
        If Not lblItemID.Text = 0 Or lblItemID.Text IsNot vbNullString Then
            SqlRefresh = "select description,brand from items where itemid like @itemid"
            msgShow = False
            SqlReFill("items", Nothing, "ShowValueInTextbox", {"itemid"}, {lblItemID}, {txtProductName, txtBrand})
        End If
    End Sub
End Class