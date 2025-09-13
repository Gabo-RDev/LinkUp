import { type NextRequest, NextResponse } from "next/server"
import { addComment, getCommentsByPostId } from "@/lib/likes-comments"

export const dynamic = "force-dynamic"

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id: postId } = params
    const comments = await getCommentsByPostId(postId)

    return NextResponse.json({ comments })
  } catch (error) {
    console.error("Error fetching comments:", error)
    return NextResponse.json({ error: "Failed to fetch comments" }, { status: 500 })
  }
}

export async function POST(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id: postId } = params
    const body = await request.json()
    const { userId, description } = body

    if (!userId || !description?.trim()) {
      return NextResponse.json({ error: "User ID and description are required" }, { status: 400 })
    }

    const comment = await addComment(postId, userId, description.trim())

    return NextResponse.json(
      {
        success: true,
        comment,
      },
      { status: 201 },
    )
  } catch (error) {
    console.error("Error adding comment:", error)
    return NextResponse.json({ error: "Failed to add comment" }, { status: 500 })
  }
}
