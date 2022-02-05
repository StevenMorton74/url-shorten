interface ShortUrlProps {
    shortUrl: string,
}

function ShortUrl(props: ShortUrlProps) {
  const { shortUrl } = props;

  return (
    <div className="shortened-url">
      <p>Your short url is:</p>
      <a href={shortUrl}>{shortUrl}</a>
    </div>
  );
}

export default ShortUrl;
