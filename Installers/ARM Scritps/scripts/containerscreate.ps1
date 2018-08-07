workflow container {
    param(
       
        [Parameter(Mandatory=$true)]
        [string]
        $destContainer1,

        [Parameter(Mandatory=$true)]
        [string]
        $destContainer2,

        [Parameter(Mandatory=$true)]
        [string]
        $destContainer3,

        [Parameter(Mandatory=$true)]
        [string]
        $destStorageAccountName,

        [Parameter(Mandatory=$true)]
        [string]
        $destStorageAccountKey
      
    )

    InlineScript{
   
        $destContainer1 = $Using:destContainer1
        $destContainer2 = $Using:destContainer2
        $destContainer3 = $Using:destContainer3
        $destStorageAccountName = $Using:destStorageAccountName
        $destStorageAccountKey = $Using:destStorageAccountKey
        
        Write-Output $destStorageAccountName,
      
        Write-Output $destContainer1
        Write-Output $destContainer2
        Write-Output $destContainer3

        $destStorage1 = New-AzureStorageContext -StorageAccountName $destStorageAccountName -StorageAccountKey $destStorageAccountKey
        New-AzureStorageContainer -Name $destContainer1 -Context $destStorage1 -Permission Blob

        $destStorage2 = New-AzureStorageContext -StorageAccountName $destStorageAccountName -StorageAccountKey $destStorageAccountKey
        New-AzureStorageContainer -Name $destContainer2 -Context $destStorage2 -Permission Blob

        $destStorage3 = New-AzureStorageContext -StorageAccountName $destStorageAccountName -StorageAccountKey $destStorageAccountKey
        New-AzureStorageContainer -Name $destContainer3 -Context $destStorage3 -Permission Blob

    }
   
}
