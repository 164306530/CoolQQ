Public Sub SougouOCR(ImagePath As String)
        Dim Cookie = New CookieContainer()
        Dim url = "http://pic.sogou.com/pic/upload_pic.jsp"
        'Dim url = "http://ocr.shouji.sogou.com/v2/ocr/json"
        Dim Str = OCR_sougou_SogouPost(url, Cookie, OCR_sougou_Image_Bytes(ImagePath))
        Dim url2 = "http://pic.sogou.com/pic/ocr/ocrOnline.jsp?query=" + Str
        Dim refer = "http://pic.sogou.com/resource/pic/shitu_intro/word_1.html?keyword=" + Str
        Dim szReturn = OCR_sougou_SogouGet(url2, Cookie, refer)
        If szReturn = "" Then Return
        Dim jsons As JObject = JObject.Parse(szReturn)
        Dim szError = jsons.SelectToken("success").ToString
        If szError = "0" Then Return
        Dim n = jsons.SelectToken("result").Count
        Dim szRes As String = ""
        For i = 0 To n - 1
            szRes = szRes + vbNewLine + jsons.SelectToken("result(" & i & ").content").ToString.Replace(vbLf, "")
        Next
    End Sub
        
    Public Function OCR_sougou_Content_Length(img As Image) As Byte()
        On Error Resume Next
        Dim bytes = Encoding.UTF8.GetBytes("------WebKitFormBoundary8orYTmcj8BHvQpVU" & vbNewLine & "Content-Disposition: form-data; name=pic; filename=pic.jpg" & vbNewLine & "Content-Type: image/jpeg" & vbNewLine & vbNewLine)
        Dim Array = ImgToBytes(img)
        Dim bytes2 = Encoding.UTF8.GetBytes(vbNewLine & "------WebKitFormBoundary8orYTmcj8BHvQpVU--" & vbNewLine)
        Dim array2 As Byte()
        ReDim array2(bytes.Length + Array.Length + bytes2.Length)
        bytes.CopyTo(array2, 0)
        Array.CopyTo(array2, bytes.Length)
        bytes2.CopyTo(array2, bytes.Length + Array.Length)
        Return array2
    End Function