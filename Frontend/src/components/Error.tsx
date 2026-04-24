


function ErrorNotification({ message }: { message: string }) {
    return (
        <div style={{
            backgroundColor: "#ff4d4f",
            color: "white",
            padding: "10px",
            borderRadius: "6px",
            marginBottom: "10px"
        }}>
            {message}
        </div>
    );
}

export default ErrorNotification;