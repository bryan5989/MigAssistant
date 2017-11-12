Namespace Encryption

    ''' <summary>
    ''' represents Hex, Byte, Base64, or String data to encrypt/decrypt;
    ''' use the .Text property to set/get a string representation
    ''' use the .Hex property to set/get a string-based Hexadecimal representation
    ''' use the .Base64 to set/get a string-based Base64 representation
    ''' </summary>
    Public Class Data
        Private _b As Byte()
        Private _MaxBytes As Integer = 0
        Private _MinBytes As Integer = 0
        Private _StepBytes As Integer = 0

        ''' <summary>
        ''' Determines the default text encoding across ALL Data instances
        ''' </summary>
        Public Shared DefaultEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Windows-1252")

        ''' <summary>
        ''' Determines the default text encoding for this Data instance
        ''' </summary>
        Public Encoding As System.Text.Encoding = DefaultEncoding

        ''' <summary>
        ''' Creates new, empty encryption data
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Creates new encryption data with the specified byte array
        ''' </summary>
        Public Sub New(ByVal b As Byte())
            _b = b
        End Sub

        ''' <summary>
        ''' Creates new encryption data with the specified string;
        ''' will be converted to byte array using default encoding
        ''' </summary>
        Public Sub New(ByVal s As String)
            Me.Text = s
        End Sub

        ''' <summary>
        ''' Creates new encryption data using the specified string and the
        ''' specified encoding to convert the string to a byte array.
        ''' </summary>
        Public Sub New(ByVal s As String, ByVal encoding As System.Text.Encoding)
            Me.Encoding = encoding
            Me.Text = s
        End Sub

        ''' <summary>
        ''' returns true if no data is present
        ''' </summary>
        Public ReadOnly Property IsEmpty() As Boolean
            Get
                If _b Is Nothing Then
                    Return True
                End If
                If _b.Length = 0 Then
                    Return True
                End If
                Return False
            End Get
        End Property

        ''' <summary>
        ''' allowed step interval, in bytes, for this data; if 0, no limit
        ''' </summary>
        Public Property StepBytes() As Integer
            Get
                Return _StepBytes
            End Get
            Set(ByVal Value As Integer)
                _StepBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' allowed step interval, in bits, for this data; if 0, no limit
        ''' </summary>
        Public Property StepBits() As Integer
            Get
                Return _StepBytes * 8
            End Get
            Set(ByVal Value As Integer)
                _StepBytes = Value \ 8
            End Set
        End Property

        ''' <summary>
        ''' minimum number of bytes allowed for this data; if 0, no limit
        ''' </summary>
        Public Property MinBytes() As Integer
            Get
                Return _MinBytes
            End Get
            Set(ByVal Value As Integer)
                _MinBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' minimum number of bits allowed for this data; if 0, no limit
        ''' </summary>
        Public Property MinBits() As Integer
            Get
                Return _MinBytes * 8
            End Get
            Set(ByVal Value As Integer)
                _MinBytes = Value \ 8
            End Set
        End Property

        ''' <summary>
        ''' maximum number of bytes allowed for this data; if 0, no limit
        ''' </summary>
        Public Property MaxBytes() As Integer
            Get
                Return _MaxBytes
            End Get
            Set(ByVal Value As Integer)
                _MaxBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' maximum number of bits allowed for this data; if 0, no limit
        ''' </summary>
        Public Property MaxBits() As Integer
            Get
                Return _MaxBytes * 8
            End Get
            Set(ByVal Value As Integer)
                _MaxBytes = Value \ 8
            End Set
        End Property

        ''' <summary>
        ''' Returns the byte representation of the data;
        ''' This will be padded to MinBytes and trimmed to MaxBytes as necessary!
        ''' </summary>
        Public Property Bytes() As Byte()
            Get
                If _MaxBytes > 0 Then
                    If _b.Length > _MaxBytes Then
                        Dim b(_MaxBytes - 1) As Byte
                        Array.Copy(_b, b, b.Length)
                        _b = b
                    End If
                End If
                If _MinBytes > 0 Then
                    If _b.Length < _MinBytes Then
                        Dim b(_MinBytes - 1) As Byte
                        Array.Copy(_b, b, _b.Length)
                        _b = b
                    End If
                End If
                Return _b
            End Get
            Set(ByVal Value As Byte())
                _b = Value
            End Set
        End Property

        ''' <summary>
        ''' Sets or returns text representation of bytes using the default text encoding
        ''' </summary>
        Public Property Text() As String
            Get
                If _b Is Nothing Then
                    Return ""
                Else
                    '-- need to handle nulls here; oddly, C# will happily convert
                    '-- nulls into the string whereas VB stops converting at the
                    '-- first null!
                    Dim i As Integer = Array.IndexOf(_b, CType(0, Byte))
                    If i >= 0 Then
                        Return Me.Encoding.GetString(_b, 0, i)
                    Else
                        Return Me.Encoding.GetString(_b)
                    End If
                End If
            End Get
            Set(ByVal Value As String)
                _b = Me.Encoding.GetBytes(Value)
            End Set
        End Property

        ''' <summary>
        ''' Sets or returns Hex string representation of this data
        ''' </summary>
        Public Property Hex() As String
            Get
                Return Utils.ToHex(_b)
            End Get
            Set(ByVal Value As String)
                _b = Utils.FromHex(Value)
            End Set
        End Property

        ''' <summary>
        ''' Sets or returns Base64 string representation of this data
        ''' </summary>
        Public Property Base64() As String
            Get
                Return Utils.ToBase64(_b)
            End Get
            Set(ByVal Value As String)
                _b = Utils.FromBase64(Value)
            End Set
        End Property

        ''' <summary>
        ''' Returns text representation of bytes using the default text encoding
        ''' </summary>
        Public Shadows Function ToString() As String
            Return Me.Text
        End Function

        ''' <summary>
        ''' returns Base64 string representation of this data
        ''' </summary>
        Public Function ToBase64() As String
            Return Me.Base64
        End Function

        ''' <summary>
        ''' returns Hex string representation of this data
        ''' </summary>
        Public Function ToHex() As String
            Return Me.Hex
        End Function

    End Class
End NameSpace