# EmuWarface
![GitHub repo size](https://img.shields.io/github/repo-size/n1kodim/EmuWarface)

A backend server emulator for Warface written in C# that supports build `1.15000.124.34300`.

### Prerequisites
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 
- [MySQL](https://dev.mysql.com/downloads/installer/)

## Quick setup

### Installation

```sh
$ git clone https://github.com/n1kodim/EmuWarface.git
$ cd ./EmuWarface
$ ./build.cmd
```

### Run game

Create a `.bat` file in the `./Bin32Release` folder of your game client and put:
```sh
Game.exe +online_server <host> +online_server_port <port> -uid <user_id> -token <pass> +online_check_certificate 0 +online_use_protect 0

# Example
Game.exe +online_server localhost +online_server_port 5222 -uid 1 -token 12345 +online_check_certificate 0 +online_use_protect 0
```

## Special thanks
- [@rubensmesquita](https://github.com/rubensmesquita) - for develop warface-emulator
- [@seagate00](https://github.com/seagate00) - for develop WarTLS

## License

This project is licensed under the GNU Affero General Public License (AGPL), Version 3.0. A copy of the license text may be found in [LICENSE](LICENSE) or at <https://www.gnu.org/licenses/>.

Copyright &copy; 2024 n1kodim.