interface ErrorMessageProps {
    error: string,
}

function ErrorMessage(props: ErrorMessageProps) {
  const { error } = props;

  return (
    <div className="error">
      <p>{error}</p>
    </div>
  );
}

export default ErrorMessage;
