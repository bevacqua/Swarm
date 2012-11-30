The Swarm
=========

Load Testing cloud swarm tool

Configuration:

- Host the Overmind in IIS, at http://overmind.groupcommerce.com
- Host the Drone in IIS, at http://drone.groupcommerce.com

- Configure both of these in *Windows\System32\drivers\etc*:

> 127.0.0.1		overmind.groupcommerce.com  
> 127.0.0.1		drone.groupcommerce.com

- Configure the database for the Overmind, use the SchemaComparison and target local database Overmind.
- Change the connection string as need be, in *Swarm.Overmind\web.config*.

- If you are having issues with the website connecting to your database instance:
    - Go back to IIS and edit the Application Pool (it can be the same for both web applications)
	- Go to the advanced configuration of the Application Pool, and edit the Identity, under Process Model.
	- Change it to custom account, and set it using your local username and password.

- Make sure you have WCF over HTTP enabled in IIS, either in:
	- "Turn windows features on or off", 
	- or refer to http://stackoverflow.com/q/451701

Other than that, you're on your own **:-)**