Imports System.IO
Imports MaterialSkin
Public Class Form1

    Private Sub UpdateItemInDataGridView(originalItem As String, newItem As String)
        For Each row As DataGridViewRow In dgvItems.Rows
            If row.Cells("Items").Value.ToString() = originalItem Then
                row.Cells("Items").Value = newItem
                Exit For
            End If
        Next
    End Sub

    Private Sub dgvItems_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvItems.CellContentClick
        If e.ColumnIndex = dgvItems.Columns("VoidButton").Index AndAlso e.RowIndex >= 0 Then
            ' Check if there are rows in the DataGridView
            If dgvItems.Rows.Count > 0 Then
                ' Check if the clicked row is not a new row
                If Not dgvItems.Rows(e.RowIndex).IsNewRow Then
                    ' Remove the clicked row
                    dgvItems.Rows.RemoveAt(e.RowIndex)
                End If
            Else
                MessageBox.Show("No items to void.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.Blue900, Accent.Teal200, TextShade.WHITE)


        ' Add a Void column with a button
        Dim voidColumn As New DataGridViewButtonColumn()
        voidColumn.HeaderText = ""
        voidColumn.Text = "Void"
        voidColumn.Name = "VoidButton"
        voidColumn.UseColumnTextForButtonValue = True
        dgvItems.Columns.Add(voidColumn)


    End Sub
    Private Sub MaterialCard1_Paint(sender As Object, e As PaintEventArgs) Handles MaterialCard1.Paint
        Me.Text = "Dashboard"
    End Sub
    Private Sub MaterialCard5_Paint(sender As Object, e As PaintEventArgs) Handles MaterialCard5.Paint
        Me.Text = "New Order"
    End Sub

    Private Sub radiobtnWashDryYES_CheckedChanged(sender As Object, e As EventArgs) Handles radiobtnWashDryYES.CheckedChanged
        If radiobtnWashDryYES.Checked Then
            ' Set the values for Wash and Dry
            dgvItems.Rows.Add("Wash and Dry", 100.0, 0, 0)
            cbWashDry.Visible = False
        End If
    End Sub
    Private Sub radiobtnWashDryNO_CheckedChanged(sender As Object, e As EventArgs) Handles radiobtnWashDryNO.CheckedChanged
        If radiobtnWashDryNO.Checked Then
            ' Populate the combo box with WashOnly and DryOnly
            cbWashDry.Visible = True
            cbWashDry.Items.Clear()
            cbWashDry.Items.Add("Wash Only")
            cbWashDry.Items.Add("Dry Only")
            cbWashDry.SelectedIndex = 0
        End If
    End Sub
    Private Sub cbWashDry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbWashDry.SelectedIndexChanged
        ' Set the values based on the selected item in the combo box
        If cbWashDry.SelectedIndex >= 0 Then
            Dim itemName As String = cbWashDry.SelectedItem.ToString()
            Dim unitPrice As Decimal = If(itemName = "Wash Only" OrElse itemName = "Dry Only", 50.0, 0.0)

            ' Get the quantity from txtboxLaundryQty
            Dim quantity As Integer
            If Integer.TryParse(txtboxLaundryQty.Text, quantity) Then
                ' Add or update the row in the DataGridView
                Dim existingRow As DataGridViewRow = dgvItems.Rows.Cast(Of DataGridViewRow)().FirstOrDefault(Function(row) row.Cells("Items").Value.ToString() = itemName)

                If existingRow IsNot Nothing Then
                    ' Update existing row
                    existingRow.Cells("Quantity").Value = quantity
                Else
                    ' Add new row
                    dgvItems.Rows.Add(itemName, unitPrice, quantity, 0)
                End If
            Else
                MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub
    Private Sub radiobtnWithSoapYes_CheckedChanged(sender As Object, e As EventArgs) Handles radiobtnWithSoapYES.CheckedChanged
        If radiobtnWithSoapYES.Checked Then
            labelWithSoapQty.Visible = True
            txtboxWithSoapQty.Visible = True
        Else
            labelWithSoapQty.Visible = False
            txtboxWithSoapQty.Visible = False
        End If
    End Sub

    Private Sub radiobtnWithFabconYes_CheckedChanged(sender As Object, e As EventArgs) Handles radiobtnWithFabconYES.CheckedChanged
        If radiobtnWithFabconYES.Checked Then
            labelWithfabconQty.Visible = True
            txtboxWithFabcoonQty.Visible = True
        Else
            labelWithfabconQty.Visible = False
            txtboxWithFabcoonQty.Visible = False
        End If
    End Sub

    Private Sub btnProceed_Click(sender As Object, e As EventArgs) Handles btnProceed.Click
        ' Process the items and calculate the total

        Dim total As Decimal = 0

        For Each row As DataGridViewRow In dgvItems.Rows
            Dim itemName As String = row.Cells("Items").Value.ToString()
            Dim unitPrice As Decimal = Convert.ToDecimal(row.Cells("UnitPrice").Value)
            Dim quantity As Integer = Convert.ToInt32(row.Cells("Quantity").Value)
            Dim subtotal As Decimal = unitPrice * quantity

            ' Update Subtotal column
            row.Cells("Subtotal").Value = subtotal

            ' Calculate total
            total += subtotal
        Next

        ' Display the total
        MessageBox.Show($"Total: {total:C}", "Total Amount", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub txtboxLaundryQty_TextChanged(sender As Object, e As EventArgs) Handles txtboxLaundryQty.TextChanged
        ' Update quantity based on the value entered in txtboxLaundryQty
        Dim newQuantity As Integer = 0
        If Integer.TryParse(txtboxLaundryQty.Text, newQuantity) Then
            dgvItems.Rows(0).Cells("Quantity").Value = newQuantity

        End If
    End Sub
End Class
