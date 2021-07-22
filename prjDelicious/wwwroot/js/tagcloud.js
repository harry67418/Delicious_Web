const fragT = $(document.createDocumentFragment())
showcloud()
async function showcloud() {
    let response = await fetch('/Search/GetHashTags/')
    let tags = await response.json()
    tags = tags.sort(function (a, b) {
        return 0.5 - Math.random()
    })
    const colors = ['darkred', 'darkgreen', 'darkblue', 'darkgoldenrod', 'dimgray']
    tags.forEach(items => {
        const name = items.hasgtag
        const size = (items.views / 100) + 30
        const color = colors[getRandom(0, 5)]
        const cloud = $('<span id="' + items.hashtagId + '" class="tagrow" style="font-size:' + size + 'px; color:' + color + ' ">#' + name + '</span>')
        if (items.views != 0) {
            fragT.append(cloud)
        }
    })
    $('#divtag').html(fragT)
    $(".tagrow").click(function () {
        let id = $(this).attr('id')
        tagclick(id)
    })
}
let tagclick = function (id) {
    window.location.href = ('/Search/TagList?id=' + id)
}
function getRandom(min, max) {
    return Math.floor(Math.random() * max) + min;
};