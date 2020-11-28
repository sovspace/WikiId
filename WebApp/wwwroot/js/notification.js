
const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl("/notification")
            .build();


hubConnection.on("Notify", function (message) {
    toastr.info(message);
});

hubConnection.start();