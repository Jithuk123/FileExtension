using System;
using System.Windows;
using Cryptlex;
using MVVM.ViewModel;

namespace MVVM.View
{
    /// <summary>

    /// </summary> 
    public partial class FileSelect : Window
    {
        public FileSelect()
        {
            InitializeComponent();

            // static void Init()
            {    
                LexActivator.SetProductData("MzYxNkQ5NjA0RDNFMDlCQUYzOUE2NUFFMkFFRjQ0QUY=.iddZNnzkZm2fGThTnoL+CVtXmGAXglwfTT+aQSDbXDkb//UuHxiVX59GiQmQUYmq7fB6S5kalNPUVI13abCPDuBaZDsm2hSBoXN0rcpLx0dvlke+3Z7nrvPcWE/h/Qg7CONHi+iYZQEhep+FEvL8Q56TDQlot7BM+hw/oBlytatJecW+V10WRixaSQZfjcEaFZ+Zb9/C7kEtnqooj2Rj7jv2bG/zz44FBSSdgLuo3mvq1jWBGsIuydrpSFgW/Q6tfeXb2cgOpJWZvmBvXzU9+L170KA1b8fMpwfrh+DHdAefzcq0hZ3Q1APi7rS3kPkB70zlaJPdttC5eP6dhX9aF6hink5rozQnG+22i9MC2DlPlMp9Jqtb6WGbZM3KKG+ndKA6PN5J3AekHRd+nHx5vmBWMKK0SgsTxPwJlvdlFyY3KEkfl+REN2HQHMGtkoZz4LIE5hZHZgm3TJBE83I6FdpZAarl1FpHm3XKdjk4KUjkbmwnPntBj+ONl7jzXfFEABkgtL7cGnWH97Ux91UY1APoijbyMsDSt++AXE1bycFowNyg2HMTv5EC1k2i5/d7bABAOMnJUdQpmoeCIgytgqYzv8G3W0Yj4qEOBiw+Dkjou/5di8T0ABK7Os8lDoabvO/2LpL6FpPAz3snOI9JKbwFHc6Al4HfxJUeWo4MoFkl1YSzVJTHcmHFuPMOnny+wiW0hp3kT2YwbbtjGL56aJtJ9KLOYJfnxNo4jWzZBXA=");
                LexActivator.SetProductId("5fb812ab-b110-44e4-96f8-409144873c3b", LexActivator.PermissionFlags.LA_USER);
                try
                {
                    Activate();
                    LexActivator.SetLicenseCallback(LicenseCallback);
                    int status = LexActivator.IsLicenseGenuine();

                    Console.WriteLine("status{0}", status);
                    Console.WriteLine("ok{0}", LexStatusCodes.LA_OK);
                    if (LexStatusCodes.LA_OK == status)
                    {
                        Console.WriteLine("License is genuinely activated!");
                        uint expiryDate = LexActivator.GetLicenseExpiryDate();
                        int daysLeft = (int)(expiryDate - DateTimeOffset.Now.ToUnixTimeSeconds()) / 86400;
                        Console.WriteLine("Days left:" + daysLeft);

                        FileSelectViewModel fileSelectViewModel = new FileSelectViewModel();
                        this.DataContext = fileSelectViewModel;
                    }
                    else if (LexStatusCodes.LA_EXPIRED == status)
                    {
                        Console.WriteLine("License is genuinely activated but has expired!");
                    }
                    else if (LexStatusCodes.LA_GRACE_PERIOD_OVER == status)
                    {
                        Console.WriteLine("License is genuinely activated but grace period is over!");
                    }
                    else if (LexStatusCodes.LA_SUSPENDED == status)
                    {
                    MessageBox.Show("License is genuinely activated but has been suspended!");
                    }
                    else
                    {
                        int trialStatus;
                        trialStatus = LexActivator.IsTrialGenuine();
                        if (LexStatusCodes.LA_OK == trialStatus)
                        {
                            uint trialExpiryDate = LexActivator.GetTrialExpiryDate();
                            int daysLeft = (int)(trialExpiryDate - DateTimeOffset.Now.ToUnixTimeSeconds()) / 86400;
                            //                 FileSelectViewModel fileSelectViewModel = new FileSelectViewModel();

                            // this.DataContext = fileSelectViewModel;
                            Console.WriteLine("Trial days left: " + daysLeft);
                        }
                        else if (LexStatusCodes.LA_TRIAL_EXPIRED == trialStatus)
                        {
                            Console.WriteLine("Trial has expired!");

                            // Time to buy the product key and activate the app
                            Activate();
                        }
                        else
                        {
                            //  Activate();
                            Console.WriteLine("Either trial has not started or has been tampered!");
                            // Activating the trial
                            trialStatus = LexActivator.ActivateTrial(); // Ideally on a button click inside a dialog
                            if (LexStatusCodes.LA_OK == trialStatus)
                            {
                                uint trialExpiryDate = LexActivator.GetTrialExpiryDate();
                                int daysLeft = (int)(trialExpiryDate - DateTimeOffset.Now.ToUnixTimeSeconds()) / 86400;
                                Console.WriteLine("Trial days left: " + daysLeft);
                            }
                            else
                            {
                                // Trial was tampered or has expired
                                Console.WriteLine("Trial activation failed: " + trialStatus);
                            }
                        }
                    }

                }
                catch (LexActivatorException ex)
                {
                    Console.WriteLine("Error code: " + ex.Code.ToString() + " Error message: " + ex.Message);
                }
                Console.WriteLine("Press any key to exit");
                // Console.ReadKey();


                static void Activate()
                {
                    LexActivator.SetLicenseKey("3BEF0C-E3D30F-41668C-0C2FB5-7E648D-2198EC");
                    LexActivator.SetActivationMetadata("key1", "value1");
                    int status = LexActivator.ActivateLicense();
                    if (LexStatusCodes.LA_OK == status || LexStatusCodes.LA_EXPIRED == status || LexStatusCodes.LA_SUSPENDED == status)
                    {
                       MessageBox.Show("License activated successfully: "+status );
                    }
                    else
                    {
                        Console.WriteLine("License activation failed: ", status);
                    }
                }

                // License callback is invoked when LexActivator.IsLicenseGenuine() completes a server sync
                static void LicenseCallback(uint status)
                {
                    // NOTE: Don't invoke IsLicenseGenuine(), ActivateLicense() or ActivateTrial() API functions in this callback
                    switch (status)
                    {
                        case LexStatusCodes.LA_SUSPENDED:
                            MessageBox.Show("The license has been suspended.");
                            break;
                        default:
                            Console.WriteLine("License status code: " + status.ToString());
                            break;
                    }
                }
            }


        }
    }

}

