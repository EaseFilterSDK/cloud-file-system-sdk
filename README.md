# EaseFilter Cloud File System SDK

[![Platform](https://img.shields.io/badge/Platform-Windows-blue.svg)]()
[![Language](https://img.shields.io/badge/Language-C%23%20%7C%20C++%20%7C%20Python%20%7C%20Rust%20%7C%20Java-success.svg)]()
[![License](https://img.shields.io/badge/License-Commercial-orange.svg)]()

EaseFilter Cloud File System is a Windows virtual file system developed using a kernel-level file system filter driver. It transparently integrates cloud storage into on-premise storage environments, allowing on-premise applications to access cloud files exactly as they would access regular physical files—without requiring any application modifications.

<Image alt="EaseFilter Cloud File System Architecture showing local and cloud storage integration" caption="Cloud File System Integration" src="https://www.easefilter.com/images/cloud-file-dr-architecture.jpg"/>

## 🚀 Key Capabilities

- **Transparent Access**: Read, write, create, and delete cloud files logically within local directories for intuitive access.
- **Active-Active Clustering**: Run applications across different servers to split traffic loads natively without extra hardware.
- **Multi-Language Support**: Includes pre-built templates for **C#, C++, Python, Rust, Java, and Go**.
- **Zero-Downtime DR**: Automatically failover local storage to the cloud in the event of a disaster with minimal business disruption.

---

## 🏗️ Architecture & Core Solutions

### 1. Load Balancing & Local Caching
The SDK optimizes performance and fault tolerance by controlling traffic across a remote storage cluster while maintaining lightning-fast local caches.

**How the Data Flows:**
1. **Request:** Remote clients send initial file reads/writes.
2. **Distribution:** A Load Balancing Gateway distributes requests across a Storage Server Cluster.
3. **Cache Check:** Each server's CloudFile Driver checks the Local File System (LFS) cache first.
4. **Data Tiering:** If the data isn't local, the driver transparently recalls the required data object and metadata (like ACLs) from the remote cloud storage back to the LFS.

### 2. Cloud-Based Disaster Recovery (Cloud DR)
Cloud-based Disaster Recovery (Cloud DR) takes a very different approach for disaster recovery based on virtualization. EaseFilter Cloud File System automatically integrates the cloud storage into the on-premies storage, it can deliver a fully functional virtual infrastructure ready to take over an outage disrupt primary services. The virtual file is stored in an offsite public cloud where it remains in standby ready for failover. Data synchronization operates in the background to keep data up-to-date. The entire system can be spun up in minutes, saving the organization from costly service interruption.

Active-Active Cluster system. With the Cloud File File System, you can set up two or more active systems, which the virtual files can be accessed from all the active systems, it can distribute your load balance and support the automated fail over without your interference.

Zero downtime for the disaster recovery. With the Cloud File DR solution, in the event of a disaster, you can automatically fail over the local storage to the cloud storage if disaster strikes,you can quickly launch resources to ensure your business continuity. You can leverage for your disaster recovery (DR) processes to significantly minimize the impact on your data, your system, and your overall business operations, your business won't be interrupted. Retrieve back the files from cloud storage is very fast, only the applications requested data will be downloaded.

## EaseFilter File System Filter Driver SDK Reference
| Product Name | Description |
| --- | --- |
| [Cloud File System SDK](https://www.easefilter.com/cloud/cloud-file-system-sdk.htm) | EaseFilter Cloud File System SDK Introduction. |
| [CloudTier Storage Tiering SDK](https://www.easefilter.com/cloud/storage-tiering-sdk.htm) | EaseFilter Storage Tiering Filter Driver SDK Introduction. |
| [File Monitor SDK](https://www.easefilter.com/kb/file-monitor-filter-driver-sdk.htm) | EaseFilter File Monitor Filter Driver SDK Introduction. |
| [File Control SDK](https://www.easefilter.com/kb/file-control-file-security-sdk.htm) | EaseFilter File Control Filter Driver SDK Introduction. |
| [File Encryption SDK](https://www.easefilter.com/kb/transparent-file-encryption-filter-driver-sdk.htm) | EaseFilter Transparent File Encryption Filter Driver SDK Introduction. |
| [Registry Filter SDK](https://www.easefilter.com/kb/registry-filter-drive-sdk.htm) | EaseFilter Registry Filter Driver SDK Introduction. |
| [Process Filter SDK](https://www.easefilter.com/kb/process-filter-driver-sdk.htm) | EaseFilter Process Filter Driver SDK Introduction. |
| [EaseFilter SDK Programming](https://www.easefilter.com/kb/programming.htm) | EaseFilter Filter Driver SDK Programming. |

## EaseFilter SDK Sample Projects
| Sample Project | Description |
| --- | --- |
| [CloudTier Storage Tiering Demo](https://www.easefilter.com/cloud/cloudtier-storage-tiering-demo.htm) | A HSM File System Filter Driver Demo. |
| [CloudTier S3 Tiering Demo](https://www.easefilter.com/cloud/cloudtier-s3-intelligent-tiering-demo.htm) | CloudTier S3 Intelligent Tiering Demo. |
| [Cloud File DR S3 Demo](https://www.easefilter.com/cloud/cloud-file-dr-demo.htm) | Cloud File DR S3 Demo. |
| [Amazon S3 File Explorer Demo](https://www.easefilter.com/cloud/s3-browser-demo.htm) | Amazon S3 File Explorer Demo. |
| [Auto File DRM Encryption](https://www.easefilter.com/kb/auto-file-drm-encryption-tool.htm) | Auto file encryption with DRM data embedded. |
| [Transparent File Encrypt](https://www.easefilter.com/kb/AutoFileEncryption.htm) | Transparent on access file encryption. |
| [Secure File Sharing with DRM](https://www.easefilter.com/kb/DRM_Secure_File_Sharing.htm) | Secure encrypted file sharing with digital rights management. |
| [File Monitor Example](https://www.easefilter.com/kb/file-monitor-demo.htm) | Monitor file system I/O in real time, tracking file changes. |
| [File Protector Example](https://www.easefilter.com/kb/file-protector-demo.htm) | Prevent sensitive files from being accessed by unauthorized users or processes. |
| [FolderLocker Example](https://www.easefilter.com/kb/FolderLocker.htm) | Lock file automatically in a FolderLocker. |
| [Process Monitor](https://www.easefilter.com/kb/Process-Monitor.htm) | Monitor the process creation and termination, block unauthorized process running. |
| [Registry Monitor](https://www.easefilter.com/kb/RegMon.htm) | Monitor the Registry activities, block the modification of the Registry keys. |
| [Secure Sandbox Example](https://www.easefilter.com/kb/Secure-Sandbox.htm) |A secure sandbox example, block the processes accessing the files out of the box. |
| [FileSystemWatcher Example](https://www.easefilter.com/kb/FileSystemWatcher.htm) | File system watcher, logging the file I/O events. |
| [ZeroTrust Example](https://www.easefilter.com/kb/zero-trust-file-access-control-demo.htm) | Zero trust file access control with encryption feature. |

## Filter Driver Reference

* [Understand MiniFilter Driver](https://www.easefilter.com/kb/understand-minifilter.htm)
* [Understand File I/O](https://www.easefilter.com/kb/File_IO.htm)
* [Understand I/O Request Packets(IRPs)](https://www.easefilter.com/kb/understand-irps.htm)
* [Filter Driver Developer Guide](https://www.easefilter.com/kb/DeveloperGuide.htm)
* [MiniFilter Filter Driver Framework](https://www.easefilter.com/kb/minifilter-framework.htm)
* [Isolation Filter Driver](https://www.easefilter.com/kb/Isolation_Filter_Driver.htm)

## License

This repository contains two distinct licensing categories:

1. **Wrapper/Implementation Code (MIT License):** 
   The source code, scripts, and configuration files provided directly in this repository are licensed under the [MIT License](LICENSE). You are free to modify, distribute, and use this specific integration code.

2. **EaseFilter SDK (Commercial License):**
   This project requires the EaseFilter SDK (including its binaries, filter drivers, and core libraries) to function. The EaseFilter SDK is **proprietary commercial software** and is strictly excluded from the MIT License.   
   To use the EaseFilter SDK for commercial purposes or in a production environment, you must obtain a valid commercial license directly from [EaseFilter](https://www.easefilter.com/license.pdf).

## Support
If you have questions or need help, please contact support@easefilter.com 

[Home](https://www.easefilter.com/) | [Solution](https://www.easefilter.com/solutions.htm) | [Download](https://www.easefilter.com/download.htm) | [Demos](https://www.easefilter.com/online-fileio-test.aspx) | [Blog](https://blog.easefilter.com/) | [Programming](https://www.easefilter.com/kb/programming.htm)
